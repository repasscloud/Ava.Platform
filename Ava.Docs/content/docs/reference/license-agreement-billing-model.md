---
title: "💳 License Agreement Billing Model"
description: "This document outlines the core billing configuration model used for customer license agreements and ongoing billing in the platform. It supports prepaid and postpaid billing, late fee logic, discounts, multiple payment methods, and international currency/tax scenarios."
summary: ""
date: 2025-04-07T05:27:35Z
lastmod: 2025-04-07T05:27:35Z
draft: false
weight: 999
toc: true
seo:
  title: "" # custom title (optional)
  description: "" # custom description (recommended)
  canonical: "" # custom canonical URL (optional)
  noindex: false # false (default) or true
---

## 📦 LicenseAgreement Class

Represents the billing configuration for a customer license or subscription.

```csharp
public class LicenseAgreement
{
    [Key]
    public int Id { get; set; }
    public PaymentTerms PaymentTerms { get; set; }               // Invoice due terms (Net 0, 30, 60, 90)
    public PaymentMethod PaymentMethod { get; set; }             // Payment method (Invoice, PayPal, etc.)
    public BillingType BillingType { get; set; }                 // Prepaid or postpaid
    public BillingFrequency BillingFrequency { get; set; }       // Monthly, Quarterly, Annually
    public bool AutoRenew { get; set; }                          // Should automatically renew
    public int GracePeriodDays { get; set; }                     // Grace period before account suspension
    public decimal PrepaidBalance { get; set; }                  // Tracks remaining prepaid credit
    public PaymentStatus PaymentStatus { get; set; }             // Current payment state
    public LateFeeConfig LateFeeConfig { get; set; }             // Optional late fee policy
    public string CurrencyCode { get; set; } = "AUD";            // Currency used for billing
    public decimal TaxRate { get; set; } = 0.10m;                // Tax rate (e.g., 10% GST)
    public DateTime? TrialEndsOn { get; set; }                   // Optional trial expiry date
    public Discount? Discount { get; set; }                      // Optional discount or promotion
}
```

## 🧾 Enums

### PaymentTerms

```csharp
public enum PaymentTerms
{
    Net0 = 0,
    Net30 = 30,
    Net60 = 60,
    Net90 = 90
}
```

### PaymentMethod

```csharp
public enum PaymentMethod
{
    Invoice,
    PayPal,
    Stripe,
    BankTransfer,
    CreditCard,
    ApplePay,
    GooglePay
}
```

### BillingType

```csharp
public enum BillingType
{
    Prepaid,
    Postpaid
}
```

### BillingFrequency

```csharp
public enum BillingFrequency
{
    Monthly,
    Quarterly,
    Annually
}
```

### PaymentStatus

```csharp
public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Overdue,
    Cancelled
}
```

### RecurringLateFeeOption

```csharp
public enum RecurringLateFeeOption
{
    None,
    Daily,
    Weekly
}
```

## 🕒 LateFeeConfig

Defines how late fees are applied to overdue invoices.

```csharp
public class LateFeeConfig
{
    public int GracePeriodDays { get; set; } = 0;                      // Days to wait after due date
    public bool UseFixedAmount { get; set; } = true;                  // Use flat fee or percent-based
    public decimal FixedAmount { get; set; } = 0m;                    // Flat late fee value
    public decimal PercentOfInvoice { get; set; } = 0m;               // % of invoice total
    public RecurringLateFeeOption RecurringOption { get; set; } = 
        RecurringLateFeeOption.None;                                  // Frequency of applying late fees
    public decimal? MaxLateFeeCap { get; set; } = null;               // Cap to prevent excessive charges
}
```

## 💸 Discount

Optional discount for a license or subscription.

```csharp
public class Discount
{
    public decimal PercentOff { get; set; }                           // Discount percentage
    public DateTime ValidUntil { get; set; }                          // Expiry date of the discount

    public bool IsActive => DateTime.UtcNow <= ValidUntil;
}
```

## ✅ Summary of Concepts

| Field | Type | Description |
|:------|:-----|:------------|
| PaymentTerms | enum | Net0, Net30, etc. |
| PaymentMethod | enum | Invoice, PayPal, etc. |
| BillingType | enum | Prepaid or Postpaid |
| BillingFrequency | enum | Monthly, Quarterly, Annually |
| AutoRenew | bool | Automatically renew subscription |
| GracePeriodDays | int | Days before suspension/late fee kicks in |
| PrepaidBalance | decimal | Remaining prepaid funds |
| PaymentStatus | enum | Paid, Failed, Overdue, etc. |
| LateFeeConfig | LateFeeConfig | Late payment rules |
| CurrencyCode | string | e.g., "AUD", "USD" |
| TaxRate | decimal | e.g., 0.10 for 10% GST |
| TrialEndsOn | DateTime? | Optional trial support |
| Discount | Discount | Optional active discount |