namespace Ava.Shared.Models.Policies;

public class Country
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string IsoCode { get; set; }

    [Required]
    public required string Flag { get; set; }

    // Each country is part of a continent
    public int? ContinentId { get; set; }

    [JsonIgnore]
    public Continent? Continent { get; set; }
}
