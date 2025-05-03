---
title: "Late Fee Config"
description: "Guides lead a user through a specific task they want to accomplish, often with a sequence of steps."
summary: ""
date: 2023-09-07T16:04:48+02:00
lastmod: 2023-09-07T16:04:48+02:00
draft: false
weight: 810
toc: true
seo:
  title: "" # custom title (optional)
  description: "" # custom description (recommended)
  canonical: "" # custom canonical URL (optional)
  robots: "" # custom robot tags (optional)
---

When the LateFeeConfig is generated, it requires the LicenseAgreementId to be provided. This is because the SQL engine will automatically link the LicenseAgreementId to the into the LicenseAgreement value itself.

This happens in the API layer automatically.

When using AvaTerminal2 to generate a LateFeeConfig from the LicenseAgreement page (`LIC`), it will automatically link it back to the value on creation automatically.

You can create a `LateFeeConfig` value, but the `LicenseAgreementId` must be manually pasted into the field. Failure to do so, will result the transaction being cancelled and referred to the respective error screen.
