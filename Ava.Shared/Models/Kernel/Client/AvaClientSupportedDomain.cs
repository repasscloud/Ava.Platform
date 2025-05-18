namespace Ava.Shared.Models.Kernel.Client;

[Index(nameof(SupportedEmailDomain), IsUnique = true)] // unique index in DB
public class AvaClientSupportedDomain
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(253)]
    public required string SupportedEmailDomain { get; set; } // this must be unique

    public int AvaClientId { get; set; }

    [JsonIgnore] // Prevent circular reference during serialization.
    public AvaClient? AvaClient { get; set; }

    // this is found from the client page itself, it's passed in from the admin portal
    // and it should always be linked, for other types of lookups, but it's optional here
    public string? ClientCode { get; set; }
}

// used to store which email account is associated with which license in Ava, the class does not 
// provide a function, but the service that interacts with this on behalf of the API routes will,
// it will remove any '@' symbols, and change everything to lowercase when storing, as a unique
// value, from a comma separated list string input
