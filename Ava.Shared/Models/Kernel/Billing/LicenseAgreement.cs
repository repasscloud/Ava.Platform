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
    
    [MoneyPrecision]
    public decimal PrepaidBalance { get; set; } = 0m;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    
    public required LateFeeConfig LateFeeConfig { get; set; }

    [Required]
    [CurrencyTypeValidation]
    public required string CurrencyCode { get; set; } = "AUD";

    [MarkupPrecision]
    public decimal TaxRate { get; set; } = 0m;
    public DateTime? TrialEndsOn { get; set; }
    public Discount? Discount { get; set; }

    // --- PNR Fees ---
    /// <summary>
    /// Fee charged (in CurrencyCode) per PNR (Passenger Name Record) created.
    /// </summary>
    [MoneyPrecision]
    public decimal PnrCreationFee { get; set; } = 0m;

    /// <summary>
    /// Fee charged (in CurrencyCode) per PNR modification/change.
    /// </summary>
    [MoneyPrecision]
    public decimal PnrChangeFee { get; set; } = 0m;

    // --- Flight Fees ---
    [MarkupPrecision]
    public decimal FlightMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal FlightPerItemFee { get; set; } = 0m;

    public ServiceFeeType FlightFeeType { get; set; } = ServiceFeeType.None;

    // --- Hotel Fees ---
    [MarkupPrecision]
    public decimal HotelMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal HotelPerItemFee { get; set; } = 0m;

    public ServiceFeeType HotelFeeType { get; set; } = ServiceFeeType.None;

    // --- Car Fees ---
    [MarkupPrecision]
    public decimal CarMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal CarPerItemFee { get; set; } = 0m;

    public ServiceFeeType CarFeeType { get; set; } = ServiceFeeType.None;

    // --- Rail Fees ---
    [MarkupPrecision]
    public decimal RailMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal RailPerItemFee { get; set; } = 0m;

    public ServiceFeeType RailFeeType { get; set; } = ServiceFeeType.None;

    // --- Transfer Fees ---
    [MarkupPrecision]
    public decimal TransferMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal TransferPerItemFee { get; set; } = 0m;

    public ServiceFeeType TransferFeeType { get; set; } = ServiceFeeType.None;

    // --- Activity Fees ---
    [MarkupPrecision]
    public decimal ActivityMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal ActivityPerItemFee { get; set; } = 0m;

    public ServiceFeeType ActivityFeeType { get; set; } = ServiceFeeType.None;

    // --- Travel (Catch-All) Fees ---
    [MarkupPrecision]
    public decimal TravelMarkupPercent { get; set; } = 0m;

    [MoneyPrecision]
    public decimal TravelPerItemFee { get; set; } = 0m;

    public ServiceFeeType TravelFeeType { get; set; } = ServiceFeeType.None;
}
