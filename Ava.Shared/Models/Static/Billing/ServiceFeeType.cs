namespace Ava.Shared.Models.Static.Billing;
public enum ServiceFeeType
{
    None = 0,               // No markup or per-item fee
    MarkupOnly = 1,         // Only percentage markup
    PerItemFeeOnly = 2,     // Only per-item fixed fee
    MarkupAndPerItemFee = 3 // Both markup and per-item fee
}
