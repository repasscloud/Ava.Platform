namespace Ava.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AvaClientController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/avaClient
    [HttpPost]
    public async Task<IActionResult> CreateAvaClient([FromBody] CreateAvaClientDTO dto)
    {
        // generate a brand new AvaClientID here, because this controller is a piece of shit
        string _avaClientID = Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 10);
        //string _avaClientID = Nanoid.Generate(Nanoid.Alphabets.UppercaseLettersAndDigits, 10);

        // Create the AvaClient entity.
        AvaClient client = new()
        {
            Id = 0,
            CompanyName = dto.CompanyName,
            ContactPersonFirstName = dto.ContactPersonFirstName,
            ContactPersonLastName = dto.ContactPersonLastName,
            ContactPersonPhone = dto.ContactPersonPhone,
            ContactPersonEmail = dto.ContactPersonEmail.ToLowerInvariant(),
            ContactPersonJobTitle = dto.ContactPersonJobTitle,
            BillingPersonFirstName = dto.BillingPersonFirstName,
            BillingPersonLastName = dto.BillingPersonLastName,
            BillingPersonPhone = dto.BillingPersonPhone,
            BillingPersonEmail = dto.BillingPersonEmail.ToLowerInvariant(),
            BillingPersonJobTitle = dto.BillingPersonJobTitle,
            AdminPersonFirstName = dto.AdminPersonFirstName,
            AdminPersonLastName = dto.AdminPersonLastName,
            AdminPersonPhone = dto.AdminPersonPhone,
            AdminPersonEmail = dto.AdminPersonEmail.ToLowerInvariant(),
            AdminPersonJobTitle = dto.AdminPersonJobTitle,
            CliendID = _avaClientID,
        };

        _context.AvaClients.Add(client);
        await _context.SaveChangesAsync();

        // Optionally create a default travel policy if indicated.
        if (dto.CreateDefaultTravelPolicy)
        {
            if (string.IsNullOrEmpty(dto.CompanyName))
            {
                return BadRequest("Company Name is required if CreateDefaultTravelPolicy is true.");
            }

            // Create a new TravelPolicy associated with the client.
            var defaultPolicy = new TravelPolicy
            {
                Id = Nanoid.Generate(alphabet: Nanoid.Alphabets.HexadecimalUppercase, size: 10),
                PolicyName = $"Default Policy",
                AvaClientId = client.Id,
                DefaultCurrencyCode = dto.DefaultBillingCurrency ?? "AUD",
            };

            _context.TravelPolicies.Add(defaultPolicy);
            await _context.SaveChangesAsync();

            // Update the client's default travel policy reference.
            client.DefaultTravelPolicyId = defaultPolicy.Id;
            client.DefaultTravelPolicy = defaultPolicy;
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(GetAvaClient), new { id = client.Id }, client);
    }

    // GET: api/avaClient/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAvaClient(int id)
    {
        var client = await _context.AvaClients
            .Include(c => c.DefaultTravelPolicy)
            .Include(c => c.TravelPolicies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    // GET: api/avaClient/clientID/{id}
    [HttpGet("clientID/{clientID}")]
    public async Task<IActionResult> GetAvaClientByClientId(string clientID)
    {
        var client = await _context.AvaClients
            .Include(c => c.DefaultTravelPolicy)
            .Include(c => c.TravelPolicies)
            .FirstOrDefaultAsync(c => c.CliendID == clientID);

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    // GET: api/avaClient/contactEmail/{email}
    [HttpGet("contactEmail/{email}")]
    public async Task<IActionResult> GetAvaClientByEmail(string email)
    {
        var client = await _context.AvaClients
            .Where(
                c => c.AdminPersonEmail == email.ToLowerInvariant() || 
                c.ContactPersonEmail == email.ToLowerInvariant() ||
                c.BillingPersonEmail == email.ToLowerInvariant())
            .FirstOrDefaultAsync();

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }
}
