namespace Ava.Shared.Models.Custom;

public class QantasCustomIATACode
{
    [Key]
    [JsonPropertyName("ID")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("IATA_Code")]
    public string IATACode { get; set; } = string.Empty;
}



// this particular class is a temporary custom class that will be used
// to look up values from the booking page, and return the IATA Code
// so that it can be used internally by the API to determine which
// airport the user is flying FROM and TO (and return if needed), but
// this class will not need to exist in the public build of the API as
// it will be managed better by proper lookup values (etc).
