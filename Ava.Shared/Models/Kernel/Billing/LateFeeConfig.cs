namespace Ava.Shared.Models.Kernel.Billing;

public class LateFeeConfig
{
    public int GracePeriodDays { get; set; }
    public bool UseFixedAmount { get; set; }
    public decimal FixedAmount { get; set; }
    public decimal PercentOfInvoice { get; set; }
    public RecurringLateFeeOption RecurringOption { get; set; } = RecurringLateFeeOption.None;
    public decimal? MaxLateFeeCap { get; set; }
}
