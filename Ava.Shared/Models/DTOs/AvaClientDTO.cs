namespace Ava.Shared.Models.DTOs;

public class AvaClientDTO
{
    public string ClientId { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? TaxIdType { get; set; }
    public string? TaxId { get; set; }
    public DateTime? TaxLastValidated { get; set; }
    public DateTime? LastUpdated { get; set; }
    
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public string? ContactPersonFirstName { get; set; }
    public string? ContactPersonLastName { get; set; }
    public string? ContactPersonCountryCode { get; set; }
    public string? ContactPersonPhone { get; set; }
    public string? ContactPersonEmail { get; set; }
    public string? ContactPersonJobTitle { get; set; }

    public string? BillingPersonFirstName { get; set; }
    public string? BillingPersonLastName { get; set; }
    public string? BillingPersonCountryCode { get; set; }
    public string? BillingPersonPhone { get; set; }
    public string? BillingPersonEmail { get; set; }
    public string? BillingPersonJobTitle { get; set; }

    public string? AdminPersonFirstName { get; set; }
    public string? AdminPersonLastName { get; set; }
    public string? AdminPersonCountryCode { get; set; }
    public string? AdminPersonPhone { get; set; }
    public string? AdminPersonEmail { get; set; }
    public string? AdminPersonJobTitle { get; set; }

    public string? DefaultCurrency { get; set; }
    public string? DefaultTravelPolicyId { get; set; }
    public string? LicenseAgreementId { get; set; }
}
