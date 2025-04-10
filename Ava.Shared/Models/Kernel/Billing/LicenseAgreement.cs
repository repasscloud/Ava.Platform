namespace Ava.Shared.Models.Kernel.Billing;

public class LicenseAgreement
{
    [Key]
    [MaxLength(12)]
    public string Id { get; set; } = Nanoid.Generate(size: 12);

    public BillingTerms PaymentTerms { get; set; } = BillingTerms.Net0;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Stripe;
    public BillingType BillingType { get; set; } = BillingType.Prepaid;
    public BillingFrequency BillingFrequency { get; set; } = BillingFrequency.Monthly;
    public bool AutoRenew { get; set; }
    public int GracePeriodDays { get; set; }
    public decimal PrepaidBalance { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public required LateFeeConfig LateFeeConfig { get; set; }
    
    [Required]
    [CurrencyTypeValidation]
    public required string CurrencyCode { get; set; } = "AUD";
    public decimal TaxRate { get; set; }
    public DateTime? TrialEndsOn { get; set; }
    public Discount? Discount { get; set; }
}
