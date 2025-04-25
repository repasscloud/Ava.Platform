namespace Ava.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITaxValidationService _taxValidation;
    private readonly ILoggerService _loggerService;

    public AvaClientController(
        ApplicationDbContext context,
        IJwtTokenService jwtTokenService,
        ITaxValidationService taxValidation,
        ILoggerService loggerService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _taxValidation = taxValidation;
        _loggerService = loggerService;
    }

    // POST: api/avaClient
    [HttpPost]
    public async Task<IActionResult> CreateAvaClient([FromBody] CreateAvaClientDTO dto)
    {
        await _loggerService.LogTraceAsync("Entering CreateAvaClient");
        await _loggerService.LogDebugAsync($"CreateAvaClient called with CompanyName={dto.CompanyName}, CreateDefaultTravelPolicy={dto.CreateDefaultTravelPolicy}");

        try
        {
            // generate a brand new AvaClientID
            string _avaClientID = Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 10);
            await _loggerService.LogInfoAsync($"Generated new AvaClientID: {_avaClientID}");

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
            await _loggerService.LogInfoAsync($"AvaClient created with Id: {client.Id}, ClientID: {client.ClientID}");

            // Optionally create a default travel policy if indicated.
            if (dto.CreateDefaultTravelPolicy)
            {
                await _loggerService.LogDebugAsync("CreateDefaultTravelPolicy is true; creating default TravelPolicy");
                if (string.IsNullOrEmpty(dto.CompanyName))
                {
                    await _loggerService.LogWarningAsync("CompanyName is required but was null or empty for default TravelPolicy creation");
                    return BadRequest("Company Name is required if CreateDefaultTravelPolicy is true.");
                }

                var defaultPolicy = new TravelPolicy
                {
                    Id = Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 10),
                    PolicyName = "Default Policy",
                    AvaClientId = client.Id,
                    DefaultCurrencyCode = dto.DefaultBillingCurrency ?? "AUD",
                };

                _context.TravelPolicies.Add(defaultPolicy);
                await _context.SaveChangesAsync();
                await _loggerService.LogInfoAsync($"Default TravelPolicy created with Id: {defaultPolicy.Id} for AvaClientId: {client.Id}");

                client.DefaultTravelPolicyId = defaultPolicy.Id;
                client.DefaultTravelPolicy = defaultPolicy;
                await _context.SaveChangesAsync();
                await _loggerService.LogDebugAsync("Updated AvaClient with DefaultTravelPolicyId");
            }

            return CreatedAtAction(nameof(GetAvaClient), new { id = client.Id }, client);
        }
        catch (Exception ex)
        {
            await _loggerService.LogErrorAsync($"Error in CreateAvaClient: {ex.Message}");
            await _loggerService.LogCriticalAsync($"Critical failure in CreateAvaClient: {ex}");
            return StatusCode(500, "An unexpected error occurred while creating the AvaClient.");
        }
    }

    // GET: api/avaClient/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAvaClient(int id)
    {
        await _loggerService.LogTraceAsync("Entering GetAvaClient");
        await _loggerService.LogDebugAsync($"GetAvaClient called with Id={id}");

        var client = await _context.AvaClients
            .Include(c => c.DefaultTravelPolicy)
            .Include(c => c.TravelPolicies)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            await _loggerService.LogWarningAsync($"AvaClient not found with Id: {id}");
            return NotFound();
        }

        await _loggerService.LogInfoAsync($"Retrieved AvaClient with Id: {id}");
        return Ok(client);
    }

    // GET: api/avaClient/clientID/{clientID}
    [HttpGet("clientID/{clientID}")]
    public async Task<IActionResult> GetAvaClientByClientId(string clientID)
    {
        await _loggerService.LogTraceAsync("Entering GetAvaClientByClientId");
        await _loggerService.LogDebugAsync($"GetAvaClientByClientId called with ClientID={clientID}");

        var client = await _context.AvaClients
            .Include(c => c.DefaultTravelPolicy)
            .Include(c => c.TravelPolicies)
            .FirstOrDefaultAsync(c => c.ClientID == clientID);

        if (client == null)
        {
            await _loggerService.LogWarningAsync($"AvaClient not found with ClientID: {clientID}");
            return NotFound();
        }

        await _loggerService.LogInfoAsync($"Retrieved AvaClient with ClientID: {clientID}");
        return Ok(client);
    }

    // GET: api/avaClient/contactEmail/{email}
    [HttpGet("contactEmail/{email}")]
    public async Task<IActionResult> GetAvaClientByEmail(string email)
    {
        await _loggerService.LogTraceAsync("Entering GetAvaClientByEmail");
        await _loggerService.LogDebugAsync($"GetAvaClientByEmail called with email={email}");

        var client = await _context.AvaClients
            .Where(c =>
                c.AdminPersonEmail == email.ToLowerInvariant() ||
                c.ContactPersonEmail == email.ToLowerInvariant() ||
                c.BillingPersonEmail == email.ToLowerInvariant())
            .FirstOrDefaultAsync();

        if (client == null)
        {
            await _loggerService.LogWarningAsync($"AvaClient not found with email: {email}");
            return NotFound();
        }

        await _loggerService.LogInfoAsync($"Retrieved AvaClient with email: {email}");
        return Ok(client);
    }

    // V1: GET by client ID
    [HttpGet("~/api/v1/avaclient/by-client-id/{clientId}")]
    public async Task<IActionResult> GetAvaClientByClientIdV1(string clientId)
    {
        await _loggerService.LogTraceAsync("Entering GetAvaClientByClientIdV1");
        await _loggerService.LogDebugAsync($"GetAvaClientByClientIdV1 called with clientId={clientId}");

        var client = await _context.AvaClients
            .Where(c => c.ClientID == clientId)
            .FirstOrDefaultAsync();

        if (client == null)
        {
            await _loggerService.LogWarningAsync($"AvaClient V1 not found with ClientID: {clientId}");
            return NotFound();
        }

        await _loggerService.LogInfoAsync($"Retrieved AvaClient V1 with ClientID: {clientId}");
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

    // V1: Create or update
    [HttpPost("~/api/v1/avaclient/new-or-update")]
    public async Task<IActionResult> CreateOrUpdateAvaClientV1([FromBody] AvaClientDTO dto)
    {
        await _loggerService.LogTraceAsync("Entering CreateOrUpdateAvaClientV1");
        await _loggerService.LogDebugAsync($"CreateOrUpdateAvaClientV1 called with ClientId={dto.ClientId}");

        var existingClient = await _context.AvaClients
            .Where(c => c.ClientID == dto.ClientId)
            .FirstOrDefaultAsync();

        if (existingClient == null)
        {
            await _loggerService.LogInfoAsync($"No existing client—creating new AvaClient with ClientID: {dto.ClientId}");
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
                BillingPersonEmail = dto.BillingPersonEmail ?? "nobody@example.com",
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

            if (!string.IsNullOrEmpty(dto.TaxID) && !string.IsNullOrEmpty(dto.Country))
            {
                newClient.TaxLastValidated = await _taxValidation.ValidateTaxRegistrationAsync(dto.TaxID, dto.Country);
                await _loggerService.LogDebugAsync($"Tax validation result for {dto.TaxID}, {dto.Country}: {newClient.TaxLastValidated}");
            }

            await _context.AvaClients.AddAsync(newClient);
            await _context.SaveChangesAsync();
            await _loggerService.LogInfoAsync($"New AvaClient created with Id: {newClient.Id}, ClientID: {newClient.ClientID}");
            return Ok();
        }
        else
        {
            await _loggerService.LogInfoAsync($"Updating existing AvaClient with Id: {existingClient.Id}, ClientID: {existingClient.ClientID}");
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
                await _loggerService.LogDebugAsync($"Tax validation result for update {dto.TaxID}, {dto.Country}: {existingClient.TaxLastValidated}");
            }

            await _context.SaveChangesAsync();
            await _loggerService.LogInfoAsync($"AvaClient updated with Id: {existingClient.Id}");
            return Ok();
        }
    }

    private async Task<(bool IsValid, IActionResult? ErrorResult)> ValidateBearerTokenAsync()
    {
        await _loggerService.LogTraceAsync("Entering ValidateBearerTokenAsync");

        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrWhiteSpace(authHeader))
            {
                await _loggerService.LogWarningAsync("Missing Authorization header");
                return (false, Unauthorized("Missing Authorization header"));
            }

            var bearerToken = authHeader.ToString();
            if (!bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await _loggerService.LogErrorAsync("Invalid token format in Authorization header");
                return (false, Unauthorized("Invalid token format"));
            }

            bearerToken = bearerToken["Bearer ".Length..].Trim();

            bool tokenValid = await _jwtTokenService.ValidateTokenAsync(jwtToken: bearerToken);
            if (!tokenValid)
            {
                await _loggerService.LogErrorAsync("Invalid or expired token");
                return (false, Unauthorized("Invalid or expired token"));
            }

            await _loggerService.LogInfoAsync("Bearer token validated successfully");
            return (true, null);
        }
        catch (Exception ex)
        {
            await _loggerService.LogCriticalAsync($"Exception in ValidateBearerTokenAsync: {ex}");
            throw;
        }
    }
}
