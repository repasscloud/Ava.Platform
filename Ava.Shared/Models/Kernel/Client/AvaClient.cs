namespace Ava.Shared.Models.Kernel.Client;

public class AvaClient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string CliendID { get; set; }

    [Required]
    public required string CompanyName { get; set; }

    [Required]
    public required string ContactPersonFirstName { get; set; }

    [Required]
    public required string ContactPersonLastName { get; set; }

    [Required]
    public required string ContactPersonPhone { get; set; }

    [Required]
    public required string ContactPersonEmail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContactPersonJobTitle { get; set; }

    [Required]
    public required string BillingPersonFirstName { get; set; }

    [Required]
    public required string BillingPersonLastName { get; set; }

    [Required]
    public required string BillingPersonPhone { get; set; }

    [Required]
    public required string BillingPersonEmail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BillingPersonJobTitle { get; set; }

    [Required]
    public required string AdminPersonFirstName { get; set; }

    [Required]
    public required string AdminPersonLastName { get; set; }

    [Required]
    public required string AdminPersonPhone { get; set; }

    [Required]
    public required string AdminPersonEmail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminPersonJobTitle { get; set; }

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

    [StringLength(3)]
    [CurrencyTypeValidation]
    public required string DefaultCurrency { get; set; }
    
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
