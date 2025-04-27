namespace Ava.Shared.Models.Kernel.Client;

public class AvaClient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string ClientId { get; set; }

    [Required]
    public required string CompanyName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TaxIdType { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TaxId { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? TaxLastValidated { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // address
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressLine1 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressLine2 { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AddressLine3 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? City { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? State { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PostalCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Country { get; set; }
    
    // contact person
    [Required]
    public required string ContactPersonFirstName { get; set; }

    [Required]
    public required string ContactPersonLastName { get; set; }

    [Required]
    public required string ContactPersonCountryCode { get; set; }

    [Required]
    public required string ContactPersonPhone { get; set; }

    [Required]
    public required string ContactPersonEmail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContactPersonJobTitle { get; set; }

    // billing person
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonFirstName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonLastName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonCountryCode { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonPhone { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonEmail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonJobTitle { get; set; }

    // admin person
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonFirstName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonLastName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonCountryCode { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonPhone { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonEmail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonJobTitle { get; set; }
    
    // financial
    [StringLength(3)]
    [CurrencyTypeValidation]
    public required string DefaultCurrency { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LicenseAgreementId { get; set; }
    
    // policies
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultTravelPolicyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TravelPolicy? DefaultTravelPolicy { get; set; }
    
    // A company can have several travel policies
    public ICollection<TravelPolicy> TravelPolicies { get; set; } = new List<TravelPolicy>();

    // Navigation property for related users (optional)
    public ICollection<AvaUser> Users { get; set; } = new List<AvaUser>();
}
