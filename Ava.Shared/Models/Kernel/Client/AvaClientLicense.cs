namespace Ava.Shared.Models.Kernel.Client;

public class AvaClientLicense
{
    [Key]
    public string Id { get; set; } = Nanoid.Generate();
    
    [Column(TypeName = "varchar(20)")] // Ensures string storage
    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required DateTime ExpiryDate { get; set; }

    [Column(TypeName = "varchar(20)")] // Ensures string storage
    [Required]
    public required string AppId { get; set; }

    public string SupportedEmailDomains { get; set; } = string.Empty;  // which domains are automatically assigned to this account, use a comma separated list
    
    public string Signature { get; set; } = string.Empty;
    
    [Required]
    public int SpendThreshold { get; set; } // what is the spend threshold of this particular client

    [Required]
    public required string IssuedBy { get; set; }

    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow; // when the license was generated
}
