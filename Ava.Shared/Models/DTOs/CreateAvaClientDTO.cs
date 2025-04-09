namespace Ava.Shared.Models.DTOs;

public class CreateAvaClientDTO
{
    public required string CompanyName { get; set; }
    public required string ContactPersonFirstName { get; set; }
    public required string ContactPersonLastName { get; set; }
    public required string ContactPersonPhone { get; set; }
    public required string ContactPersonEmail { get; set; }
    public string? ContactPersonJobTitle { get; set; }
    public required string BillingPersonFirstName { get; set; }
    public required string BillingPersonLastName { get; set; }
    public required string BillingPersonPhone { get; set; }
    public required string BillingPersonEmail { get; set; }
    public string? BillingPersonJobTitle { get; set; }
    public required string AdminPersonFirstName { get; set; }
    public required string AdminPersonLastName { get; set; }
    public required string AdminPersonPhone { get; set; }
    public required string AdminPersonEmail { get; set; }
    public string? AdminPersonJobTitle { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [CurrencyTypeValidation]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be exactly 3 uppercase letters.")]
    [DefaultValue("AUD")]
    public string? DefaultBillingCurrency { get; set; } = "AUD";

    // Optionally create a default travel policy.
    public bool CreateDefaultTravelPolicy { get; set; }
}
