namespace Ava.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITaxValidationService _taxValidation;

    public AvaClientController(ApplicationDbContext context, IJwtTokenService jwtTokenService, ITaxValidationService taxValidation)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _taxValidation = taxValidation;
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
            ContactPersonCountryCode = dto.ContactPersonCountryCode,
            ContactPersonPhone = dto.ContactPersonPhone,
            ContactPersonEmail = dto.ContactPersonEmail.ToLowerInvariant(),
            ContactPersonJobTitle = dto.ContactPersonJobTitle,
            BillingPersonFirstName = dto.BillingPersonFirstName,
            BillingPersonLastName = dto.BillingPersonLastName,
            BillingPersonCountryCode = dto.BillingPersonCountryCode,
            BillingPersonPhone = dto.BillingPersonPhone,
            BillingPersonEmail = dto.BillingPersonEmail.ToLowerInvariant(),
            BillingPersonJobTitle = dto.BillingPersonJobTitle,
            AdminPersonFirstName = dto.AdminPersonFirstName,
            AdminPersonLastName = dto.AdminPersonLastName,
            AdminPersonCountryCode = dto.AdminPersonCountryCode,
            AdminPersonPhone = dto.AdminPersonPhone,
            AdminPersonEmail = dto.AdminPersonEmail.ToLowerInvariant(),
            AdminPersonJobTitle = dto.AdminPersonJobTitle,
            ClientID = _avaClientID,
            DefaultCurrency = dto.DefaultBillingCurrency ?? "AUD",
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
            .FirstOrDefaultAsync(c => c.ClientID == clientID);

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
            .Where(c => 
                c.AdminPersonEmail == email.ToLowerInvariant() || 
                c.ContactPersonEmail == email.ToLowerInvariant() ||
                c.BillingPersonEmail == email.ToLowerInvariant())
            .FirstOrDefaultAsync();

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpGet("~/api/v1/avaclient/by-client-id/{clientId}")]
    public async Task<IActionResult> GetAvaClientByClientIdV1(string clientId)
    {
        var client = await _context.AvaClients
            .Where(c =>
                c.ClientID == clientId)
            .FirstOrDefaultAsync();
        
        if (client is null)
        {
            return NotFound();
        }
        else
        {
            var result = new AvaClientDTO
            {
                ClientId = client.ClientID,
                CompanyName = client.CompanyName,
                
                TaxIDType = client?.TaxIDType ?? null,
                TaxID = client?.TaxID ?? null,
                TaxLastValidated = client?.TaxLastValidated ?? null,
                
                AddressLine1 = client?.AddressLine1 ?? null,
                AddressLine2 = client?.AddressLine2 ?? null,
                AddressLine3 = client?.AddressLine3 ?? null,
                City = client?.City ?? null,
                State = client?.State ?? null,
                PostalCode = client?.PostalCode ?? null,
                Country = client?.Country ?? null,
                
                ContactPersonFirstName = client!.ContactPersonFirstName,
                ContactPersonLastName = client!.ContactPersonLastName,
                ContactPersonCountryCode = client!.ContactPersonCountryCode,
                ContactPersonPhone = client!.ContactPersonPhone,
                ContactPersonEmail = client!.ContactPersonEmail,
                ContactPersonJobTitle = client!.ContactPersonJobTitle,

                BillingPersonFirstName = client!.BillingPersonFirstName,
                BillingPersonLastName = client!.BillingPersonLastName,
                BillingPersonCountryCode = client!.BillingPersonCountryCode,
                BillingPersonPhone = client!.BillingPersonPhone,
                BillingPersonEmail = client!.BillingPersonEmail,
                BillingPersonJobTitle = client!.BillingPersonJobTitle,

                AdminPersonFirstName = client!.AdminPersonFirstName,
                AdminPersonLastName = client!.AdminPersonLastName,
                AdminPersonCountryCode = client!.AdminPersonCountryCode,
                AdminPersonPhone = client!.AdminPersonPhone,
                AdminPersonEmail = client!.AdminPersonEmail,
                AdminPersonJobTitle = client!.AdminPersonJobTitle,

                DefaultCurrency = client!.DefaultCurrency,
                DefaultTravelPolicyId = client?.DefaultTravelPolicyId ?? null,
                LicenseAgreementId = client?.LicenseAgreementId ?? null,
            };

            return Ok(result);
        }
    }

    [HttpPost("~/api/v1/avaclient/new-or-update")]
    public async Task<IActionResult> CreateOrUpdateAvaClientV1([FromBody]AvaClientDTO dto)
    {
        var existingClient = await _context.AvaClients
            .Where(c =>
                c.ClientID == dto.ClientId)
            .FirstOrDefaultAsync();
        
        if (existingClient is null)
        {
            // create new record
            AvaClient newClient = new AvaClient
            {
                Id = 0,
                ClientID = dto.ClientId,
                CompanyName = dto.CompanyName,
                TaxIDType = dto.TaxIDType ?? null,
                TaxID = dto.TaxID ?? null,
                TaxLastValidated = dto.TaxLastValidated ?? null,
                ContactPersonFirstName = dto.ContactPersonFirstName ?? "UNKNOWN",
                ContactPersonLastName = dto.ContactPersonLastName ?? "UNKNOWN",
                ContactPersonCountryCode = dto.ContactPersonCountryCode ?? string.Empty,
                ContactPersonPhone = dto.ContactPersonPhone ?? "0000000000",
                ContactPersonEmail = dto.ContactPersonEmail ?? "nobody@example.com",
                ContactPersonJobTitle = dto.ContactPersonJobTitle ?? "UNKNOWN",
                BillingPersonFirstName = dto.BillingPersonFirstName ?? "UNKNOWN",
                BillingPersonLastName = dto.BillingPersonLastName ?? "UNKNOWN",
                BillingPersonCountryCode = dto.BillingPersonCountryCode ?? string.Empty,
                BillingPersonPhone = dto.BillingPersonPhone ?? "0000000000",
                BillingPersonEmail = dto.BillingPersonEmail = "nobody@example.com",
                BillingPersonJobTitle = dto.BillingPersonJobTitle ?? "UNKNOWN",
                AdminPersonFirstName = dto.AdminPersonFirstName ?? "UNKNOWN",
                AdminPersonLastName = dto.AdminPersonLastName ?? "UNKNOWN",
                AdminPersonCountryCode = dto.AdminPersonCountryCode ?? string.Empty,
                AdminPersonPhone = dto.AdminPersonPhone ?? "UNKNOWN",
                AdminPersonEmail = dto.AdminPersonEmail ?? "nobody@example.com",
                AdminPersonJobTitle = dto.AdminPersonJobTitle ?? "UNKNOWN",
                AddressLine1 = dto.AddressLine1 ?? string.Empty,
                AddressLine2 = dto.AddressLine2 ?? string.Empty,
                AddressLine3 = dto.AddressLine3 ?? string.Empty,
                City = dto.City ?? string.Empty,
                State = dto.State ?? string.Empty,
                PostalCode = dto.PostalCode ?? string.Empty,
                Country = dto.Country ?? string.Empty,
                DefaultCurrency = dto.DefaultCurrency ?? "AUD",
                DefaultTravelPolicyId = dto.DefaultTravelPolicyId ?? string.Empty,
                LicenseAgreementId = dto.LicenseAgreementId ?? string.Empty
            };

            if (!string.IsNullOrEmpty(dto.Country) && !string.IsNullOrEmpty(dto.TaxID))
            {
                newClient.TaxLastValidated = await _taxValidation.ValidateTaxRegistrationAsync(dto.TaxID, dto.Country);
            }

            await _context.AvaClients.AddAsync(newClient);
            await _context.SaveChangesAsync();

            return Ok();
        }
        else
        {
            existingClient.CompanyName = dto.CompanyName;
            existingClient.TaxIDType = dto?.TaxIDType ?? null;
            existingClient.TaxID = dto?.TaxID ?? null;
            existingClient.TaxLastValidated = dto?.TaxLastValidated ?? null;

            existingClient.AddressLine1 = dto?.AddressLine1 ?? null;
            existingClient.AddressLine2 = dto?.AddressLine2 ?? null;
            existingClient.AddressLine3 = dto?.AddressLine3 ?? null;
            existingClient.City = dto?.City ?? null;
            existingClient.State = dto?.State ?? null;
            existingClient.PostalCode = dto?.PostalCode ?? null;
            existingClient.Country = dto?.Country ?? null;

            existingClient.ContactPersonFirstName = dto?.ContactPersonFirstName ?? "UNKNOWN";
            existingClient.ContactPersonLastName = dto?.ContactPersonLastName ?? "UNKNOWN";
            existingClient.ContactPersonCountryCode = dto?.ContactPersonCountryCode ?? "UNKNOWN";
            existingClient.ContactPersonPhone = dto?.ContactPersonPhone ?? "0000000000";
            existingClient.ContactPersonEmail = dto?.ContactPersonEmail ?? "nobody@example.com";
            existingClient.ContactPersonJobTitle = dto?.ContactPersonJobTitle ?? "UNKNOWN";

            existingClient.BillingPersonFirstName = dto?.BillingPersonFirstName ?? "UNKNOWN";
            existingClient.BillingPersonLastName = dto?.BillingPersonLastName ?? "UNKNOWN";
            existingClient.BillingPersonCountryCode = dto?.BillingPersonCountryCode ?? "UNKNOWN";
            existingClient.BillingPersonPhone = dto?.BillingPersonPhone ?? "0000000000";
            existingClient.BillingPersonEmail = dto?.BillingPersonEmail ?? "nobody@example.com";
            existingClient.BillingPersonJobTitle = dto?.BillingPersonJobTitle ?? "UNKNOWN";

            existingClient.AdminPersonFirstName = dto?.AdminPersonFirstName ?? "UNKNOWN";
            existingClient.AdminPersonLastName = dto?.AdminPersonLastName ?? "UNKNOWN";
            existingClient.AdminPersonCountryCode = dto?.AdminPersonCountryCode ?? "UNKNOWN";
            existingClient.AdminPersonPhone = dto?.AdminPersonPhone ?? "0000000000";
            existingClient.AdminPersonEmail = dto?.AdminPersonEmail ?? "nobody@example.com";
            existingClient.AdminPersonJobTitle = dto?.AdminPersonJobTitle ?? "UNKNOWN";

            existingClient.DefaultCurrency = dto?.DefaultCurrency ?? "AUD";
            existingClient.DefaultTravelPolicyId = dto?.DefaultTravelPolicyId ?? null;
            existingClient.LicenseAgreementId = dto?.LicenseAgreementId ?? null;

            if (!string.IsNullOrEmpty(dto?.Country) && !string.IsNullOrEmpty(dto?.TaxID))
            {
                existingClient.TaxLastValidated = await _taxValidation.ValidateTaxRegistrationAsync(dto.TaxID, dto.Country);
            }
            
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    private async Task<(bool IsValid, IActionResult? ErrorResult)> ValidateBearerTokenAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrWhiteSpace(authHeader))
            return (false, Unauthorized("Missing Authorization header"));

        var bearerToken = authHeader.ToString();
        if (!bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return (false, Unauthorized("Invalid token format"));

        bearerToken = bearerToken["Bearer ".Length..].Trim();

        bool tokenValid = await _jwtTokenService.ValidateTokenAsync(jwtToken: bearerToken);
        if (!tokenValid)
            return (false, Unauthorized("Invalid or expired token"));

        return (true, null);
    }
}
