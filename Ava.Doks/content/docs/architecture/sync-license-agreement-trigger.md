---
title: "Sync License Agreement Trigger"
description: "Automatically updates AvaClients.LicenseAgreementId when new LicenseAgreements are created."
summary: ""
date: 2023-09-07T16:13:18+02:00
lastmod: 2025-04-09T12:00:00+10:00
draft: false
weight: 915
toc: true
seo:
  title: "Sync License Agreement Trigger"
  description: "Internal guide for the sync_license_agreement_id Postgres trigger."
  canonical: ""
  robots: ""
---

## Goal

Ensure that whenever a new record is inserted into `public."LicenseAgreements"`, its primary key (`Id`) is propagated to the matching row in `public."AvaClients"` by updating the `LicenseAgreementId` column.

## Trigger Function

```sql
CREATE OR REPLACE FUNCTION public.sync_license_agreement_id()
RETURNS trigger AS $$
DECLARE
  updated_count INT;
BEGIN
  -- update matching client record
  UPDATE public."AvaClients"
     SET "LicenseAgreementId" = NEW."Id"
   WHERE "ClientId" = NEW."AvaClientId";

  -- emit notice if no row was updated
  GET DIAGNOSTICS updated_count = ROW_COUNT;
  IF updated_count = 0 THEN
    RAISE NOTICE 'No matching AvaClients row for ClientId=%', NEW."AvaClientId";
  END IF;

  RETURN NEW;
END;
$$
LANGUAGE plpgsql;
```

## Trigger Definition

```sql
CREATE TRIGGER trg_license_agreement_insert
  AFTER INSERT ON public."LicenseAgreements"
  FOR EACH ROW
  EXECUTE FUNCTION public.sync_license_agreement_id();
```

- **Event**: `AFTER INSERT` on `public."LicenseAgreements"`.  
- **Scope**: `FOR EACH ROW` ensures the trigger runs once per inserted row.  
- **Function**: `sync_license_agreement_id()` performs the update.

## Behavior

- On insert of a LicenseAgreement, `NEW."AvaClientId"` and `NEW."Id"` are available.  
- The trigger locates the client where `ClientId = NEW."AvaClientId"` and sets its `LicenseAgreementId`.  
- If no matching client exists, a `NOTICE` is logged but the original insert succeeds.

## Deployment

- Apply the script via psql or migration tools (e.g., Flyway, Liquibase).  
- Wrap in `IF NOT EXISTS` checks if re-running is required.

## Notes

- No errors are thrown for unmatched rows by default.  
- Replace `NOTICE` with `EXCEPTION` in the function to enforce strict referential integrity.  
- Keep the function and trigger in `public` or a dedicated schema as per team conventions.
