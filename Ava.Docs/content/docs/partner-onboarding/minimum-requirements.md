---
title: "Minimum Requirements"
description: "Minimum requirements to onboard a new partner into the Ava.Platform."
summary: ""
date: 2023-09-07T16:13:18+02:00
lastmod: 2025-04-09T12:00:00+10:00
draft: false
weight: 910
toc: true
seo:
  title: "Minimum Requirements for Partner Onboarding"
  description: "Core data and config required to onboard a partner into Ava.Platform, including contact info, domains, licenses, and payment setup."
  canonical: ""
  robots: ""
---

The JSON payload to create a new client is a DTO (`CreateAvaClientDTO`). The body consists of the following JSON:

```json
{
  "companyName": "string",
  "contactPersonFirstName": "string",
  "contactPersonLastName": "string",
  "contactPersonPhone": "string",
  "contactPersonEmail": "string",
  "contactPersonJobTitle": "string",
  "billingPersonFirstName": "string",
  "billingPersonLastName": "string",
  "billingPersonPhone": "string",
  "billingPersonEmail": "string",
  "billingPersonJobTitle": "string",
  "adminPersonFirstName": "string",
  "adminPersonLastName": "string",
  "adminPersonPhone": "string",
  "adminPersonEmail": "string",
  "adminPersonJobTitle": "string",
  "defaultBillingCurrency": "AUD",
  "createDefaultTravelPolicy": true
}

## Supported Currencies

- `AUD` Australian Dollar
- `USD` United States Dollar
- `EUR` Euro
- `GBP` Pound Sterling
- `JPY` Japanese Yen
- `CHF` Swiss Franc
- `CAD` Canadian Dollar
- `CNY` Chinese Yuan Renminbi
- `HKD` Hong Kong Dollar
- `SGD` Singapore Dollar

## Client ID

A Client ID is generated automatically, consisting of 10 uppercase alphanumeric characters. It is created using Nanoid to ensure uniqueness and avoid collisions:

```csharp
Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 10);
```

## Client Supported Domains

Each client must have **at least one email domain** associated with their account. This is done via the Ava.Terminal application:

> Clients → Domains → Add

The associated domain enables proper linkage of users to accounts for billing, policy enforcement, and access control. The `ClientID` is required for this step.

## License

A **license** must be procured and assigned before the client or their users can log in.

Setup is handled via:

> Ava.Terminal → Clients → Licenses

Without an active license, access will be denied. Licenses may include trial modes, gratis access, or full commercial contracts.

## Payment Terms & Configuration

As part of onboarding, the minimum **billing configuration** must be provided. This is required to activate the client's license and define how and when they pay for ongoing usage.

### Required Fields

| Field | Example Values | Notes |
|:------|:---------------|:------|
| PaymentTerms | Net0, Net30, Net60 | Days after invoice before payment is due |
| PaymentMethod | Stripe, PayPal, BankTransfer, Invoice | How the client will pay |
| BillingType | Prepaid, Postpaid | Whether payment is made in advance |
| BillingFrequency | Monthly, Quarterly | Interval at which the client is billed |
| CurrencyCode | "AUD", "USD" | Default currency for invoicing |
| TaxRate | 0.10 | e.g. 10% GST (based on region/jurisdiction) |
| AutoRenew | true, false | Whether the license renews automatically |

### Optional Trial or Special Status

Some accounts can be created under non-billable modes:

- `Trial90` – 90-day trial period
- `Trial120` – 120-day trial period
- `Gratis` – Free access, no billing
- `UAT` – Internal or partner testing only

These modes may bypass normal payment terms but still require license and client creation.
