---
title: "Account Financial Status: Enum Overview"
description: "Defines and explains the financial account status values used for billing and account management in Ava.Platform."
summary: ""
date: 2025-04-27T18:30:00+10:00
lastmod: 2025-04-27T18:30:00+10:00
draft: false
weight: 910
toc: true
seo:
  title: "Financial Account Status Enum Overview"
  description: "Internal guide for understanding and using the AccountFinancialStatus enum within Ava.Platform."
  canonical: ""
  robots: "index,follow"
---

## Goal

The purpose of the `AccountFinancialStatus` enum is to define a consistent way to track a client's financial standing across Ava.Platform.  
It is used for account lifecycle management, billing enforcement, and UI labeling for account status indicators.

This enum is part of the core billing domain located in:

```csharp
Ava.Shared.Models.Static.Billing
```

It applies across services like Client Management, Licensing, Billing, and Suspension Logic.

## Enum Definition

```csharp
namespace Ava.Shared.Models.Static.Billing;

public enum AccountFinancialStatus
{
    /// <summary>
    /// Account is pending activation or configuration.
    /// </summary>
    Pending,
    
    /// <summary>
    /// Account is fully up to date, no unpaid invoices.
    /// </summary>
    Current,

    /// <summary>
    /// Within allowed billing period (i.e., still inside Net30/Net60 terms but unpaid).
    /// </summary>
    InBillingPeriod,

    /// <summary>
    /// Past the due date; invoice is overdue.
    /// </summary>
    Overdue,

    /// <summary>
    /// Account has been placed on hold due to non-payment (manual or automatic).
    /// </summary>
    Suspended,

    /// <summary>
    /// Account is formally terminated (requires reactivation/new agreement).
    /// </summary>
    Terminated
}
```

## Financial Status Stages

| Status              | Meaning |
|:--------------------|:--------|
| `Pending`            | Account created but not fully activated or configured yet. |
| `Current`            | All invoices paid, no outstanding balance. |
| `InBillingPeriod`    | Invoice unpaid, but still inside agreed Net payment terms (Net30, Net60, etc.). |
| `Overdue`            | Invoice past due date, client is officially late on payment. |
| `Suspended`          | Account manually or automatically suspended due to non-payment or other reasons. |
| `Terminated`         | Account permanently closed or terminated; cannot use platform services until reactivation. |

## Evaluation Logic

Accounts should be assigned an `AccountFinancialStatus` based on the following business logic:

```csharp
if (invoice.IsPaid)
    status = AccountFinancialStatus.Current;
else if (DateTime.UtcNow <= invoice.DueDate)
    status = AccountFinancialStatus.InBillingPeriod;
else if (DateTime.UtcNow > invoice.DueDate && !account.IsSuspended)
    status = AccountFinancialStatus.Overdue;
else if (account.IsSuspended)
    status = AccountFinancialStatus.Suspended;
else if (account.IsTerminated)
    status = AccountFinancialStatus.Terminated;
else
    status = AccountFinancialStatus.Pending;
```

- If an account has **no active invoices**, assume **Pending** or **Current** depending on configuration completeness.
- **Suspended** overrides **Overdue** for display purposes.
- **Terminated** means the account should not generate or accept any billing or booking activity.

## Example Usage

When rendering account status in the Admin Panel:

```csharp
AccountFinancialStatus status = _billingService.GetAccountFinancialStatus(clientId);

switch (status)
{
    case AccountFinancialStatus.Current:
        // show green "Account in good standing"
        break;
    case AccountFinancialStatus.Overdue:
        // show red "Overdue - Immediate Action Required"
        break;
    case AccountFinancialStatus.Suspended:
        // block access and show suspension notice
        break;
}
```

Or when applying automatic holds:

```csharp
if (status == AccountFinancialStatus.Overdue && overdueDays > 14)
{
    _suspensionService.SuspendAccount(clientId);
}
```

## Notes

- This enum should always be **calculated dynamically** based on the latest invoices, suspension flags, and license status.
- Manual intervention (e.g., reactivating Suspended/Terminated accounts) should be logged and audited.
- Any service that touches billing, booking, or client visibility must respect the `AccountFinancialStatus`.
