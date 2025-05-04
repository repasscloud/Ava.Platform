using Ava.Shared.Models.Kernel.Billing;

namespace Ava.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITaxValidationService _taxValidation;
    private readonly ILoggerService _loggerService;
    private readonly ILicenseAgreementService _licenseService;
    private readonly ILateFeeConfigService   _lateFeeService;

    public AvaClientController(
        ApplicationDbContext context,
        IJwtTokenService jwtTokenService,
        ITaxValidationService taxValidation,
        ILoggerService loggerService,
        ILicenseAgreementService licenseService,
        ILateFeeConfigService lateFeeService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _taxValidation = taxValidation;
        _loggerService = loggerService;
        _licenseService = licenseService;
        _lateFeeService = lateFeeService;
    }

    // POST: api/avaClient
    [HttpPost]
    public async Task<IActionResult> CreateAvaClient([FromBody] CreateAvaClientDTO dto)
    {
        // Validate token
        var (isValid, errorResult) = await ValidateBearerTokenAsync();
        if (!isValid)
        {
            var dtoJson = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                WriteIndented = false
            });
            await _loggerService.LogWarningAsync(
                $"Unauthorized call to CreateAvaClient for '{dtoJson}'."
            );
            return errorResult!;
        }

        await _loggerService.LogTraceAsync("Entering CreateAvaClient");
        await _loggerService.LogDebugAsync($"CreateAvaClient called with CompanyName={dto.CompanyName}, CreateDefaultTravelPolicy={dto.CreateDefaultTravelPolicy}");

        try
        {
            // generate a brand new AvaClientId
            string _avaClientId = string.Empty;
            if (dto.ClientId?.Length == 0)
            {
                _avaClientId = Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 10);
                await _loggerService.LogInfoAsync($"Generated new AvaClientId: {_avaClientId}");
            }
            

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
                ContactPersonJobTitle = dto?.ContactPersonJobTitle,
                BillingPersonFirstName = dto?.BillingPersonFirstName ?? null,
                BillingPersonLastName = dto?.BillingPersonLastName ?? null,
                BillingPersonCountryCode = dto?.BillingPersonCountryCode ?? null,
                BillingPersonPhone = dto?.BillingPersonPhone ?? null,
                BillingPersonEmail = dto?.BillingPersonEmail?.ToLowerInvariant() ?? null,
                BillingPersonJobTitle = dto?.BillingPersonJobTitle ?? null,
                AdminPersonFirstName = dto?.AdminPersonFirstName ?? null,
                AdminPersonLastName = dto?.AdminPersonLastName ?? null,
                AdminPersonCountryCode = dto?.AdminPersonCountryCode ?? null,
                AdminPersonPhone = dto?.AdminPersonPhone ?? null,
                AdminPersonEmail = dto?.AdminPersonEmail?.ToLowerInvariant(),
                AdminPersonJobTitle = dto?.AdminPersonJobTitle ?? null,
                ClientId = dto?.ClientId is { Length: > 0 }
                    ? dto.ClientId
                    : _avaClientId,
                DefaultCurrency = dto?.DefaultBillingCurrency ?? "AUD",
            };

            _context.AvaClients.Add(client);
            await _context.SaveChangesAsync();
            await _loggerService.LogInfoAsync($"AvaClient created with Id: {client.Id}, ClientId: {client.ClientId}");

            // Optionally create a default travel policy if indicated.
            if (dto!.CreateDefaultTravelPolicy)
            {
                await _loggerService.LogDebugAsync("CreateDefaultTravelPolicy is true; creating default TravelPolicy");
                if (string.IsNullOrEmpty(dto.CompanyName))
                {
                    await _loggerService.LogWarningAsync("CompanyName is required but was null or empty for default TravelPolicy creation");
                    return BadRequest("Company Name is required if CreateDefaultTravelPolicy is true.");
                }

                var defaultPolicy = new TravelPolicy
                {
                    Id = Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 14),
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
        // Validate token
        var (isValid, errorResult) = await ValidateBearerTokenAsync();
        if (!isValid)
        {
            await _loggerService.LogWarningAsync(
                $"Unauthorized call to GetAvaClient for '{id}'."
            );
            return errorResult!;
        }
        
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

    // GET: api/avaClient/clientId/{clientId}
    [HttpGet("clientId/{clientId}")]
    public async Task<IActionResult> GetAvaClientByClientId(string clientId)
    {
        // Validate token
        var (isValid, errorResult) = await ValidateBearerTokenAsync();
        if (!isValid)
        {
            await _loggerService.LogWarningAsync(
                $"Unauthorized call to GetAvaClientByClientId for '{clientId}'."
            );
            return errorResult!;
        }
        
        await _loggerService.LogTraceAsync("Entering GetAvaClientByClientId");
        await _loggerService.LogDebugAsync($"GetAvaClientByClientId called with ClientId={clientId}");

        var client = await _context.AvaClients
            .Include(c => c.DefaultTravelPolicy)
            .Include(c => c.TravelPolicies)
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (client == null)
        {
            await _loggerService.LogWarningAsync($"AvaClient not found with ClientId: {clientId}");
            return NotFound();
        }

        await _loggerService.LogInfoAsync($"Retrieved AvaClient with ClientId: {clientId}");
        return Ok(client);
    }

    // GET: api/avaClient/contactEmail/{email}
    [HttpGet("contactEmail/{email}")]
    public async Task<IActionResult> GetAvaClientByEmail(string email)
    {
        // Validate token
        var (isValid, errorResult) = await ValidateBearerTokenAsync();
        if (!isValid)
        {
            await _loggerService.LogWarningAsync(
                $"Unauthorized call to GetAvaClientByEmail for '{email}'."
            );
            return errorResult!;
        }

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
        // Validate token
        var (isValid, errorResult) = await ValidateBearerTokenAsync();
        if (!isValid)
        {
            await _loggerService.LogWarningAsync(
                $"Unauthorized call to GetAvaClientByClientIdV1 for '{clientId}'."
            );
            return errorResult!;
        }

        await _loggerService.LogTraceAsync("Entering GetAvaClientByClientIdV1");
        await _loggerService.LogDebugAsync($"GetAvaClientByClientIdV1 called with clientId={clientId}");

        var client = await _context.AvaClients
            .Where(c => c.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (client == null)
        {
            await _loggerService.LogWarningAsync($"AvaClient V1 not found with ClientId: {clientId}");
            return NotFound();
        }

        await _loggerService.LogInfoAsync($"Retrieved AvaClient V1 with ClientId: {clientId}");
        var result = new AvaClientDTO
        {
            ClientId = client.ClientId,
            CompanyName = client.CompanyName,

            TaxIdType = client?.TaxIdType ?? null,
            TaxId = client?.TaxId ?? null,
            TaxLastValidated = client?.TaxLastValidated ?? null,
            LastUpdated = DateTime.UtcNow,
            
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
        // // Validate token
        // var (isValid, errorResult) = await ValidateBearerTokenAsync();
        // if (!isValid)
        // {
        //     var dtoJson = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        //     {
        //         WriteIndented = false
        //     });
        //     await _loggerService.LogWarningAsync(
        //         $"Unauthorized call to CreateOrUpdateAvaClientV1 for '{dtoJson}'."
        //     );
        //     return errorResult!;
        // }

        await _loggerService.LogTraceAsync("Entering CreateOrUpdateAvaClientV1");
        await _loggerService.LogDebugAsync($"CreateOrUpdateAvaClientV1 called with ClientId={dto.ClientId}");

        var existingClient = await _context.AvaClients
            .Where(c => c.ClientId == dto.ClientId)
            .FirstOrDefaultAsync();

        if (existingClient == null)
        {
            await _loggerService.LogInfoAsync($"No existing client—creating new AvaClient with ClientId: {dto.ClientId}");
            AvaClient newClient = new AvaClient
            {
                Id = 0,
                ClientId = dto.ClientId,
                CompanyName = dto.CompanyName,
                TaxIdType = dto.TaxIdType ?? null,
                TaxId = dto.TaxId ?? null,
                TaxLastValidated = dto.TaxLastValidated ?? null,
                LastUpdated = DateTime.UtcNow,
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
                DefaultTravelPolicyId = dto?.DefaultTravelPolicyId is { Length: > 0 }
                    ? dto?.DefaultTravelPolicyId
                    : null,
                LicenseAgreementId = dto?.LicenseAgreementId is { Length: > 0 }
                    ? dto?.LicenseAgreementId
                    : null,
            };

            if (!string.IsNullOrWhiteSpace(dto?.TaxId) && !string.IsNullOrWhiteSpace(dto?.Country))
            {
                newClient.TaxLastValidated = await _taxValidation.ValidateTaxRegistrationAsync(dto.TaxId, dto.Country);
                await _loggerService.LogDebugAsync($"Tax validation result for {dto.TaxId}, {dto.Country}: {newClient.TaxLastValidated}");
            }

            await _context.AvaClients.AddAsync(newClient);
            await _context.SaveChangesAsync();
            await _loggerService.LogInfoAsync($"New AvaClient created with Id: {newClient.Id}, ClientId: {newClient.ClientId}");

            // create the default policy
            var defaultPolicy = new TravelPolicy
            {
                Id                     = Nanoid.Generate(Nanoid.Alphabets.HexadecimalUppercase, 10),
                PolicyName             = $"{dto?.CompanyName} Default Policy",
                AvaClientId            = newClient.Id,
                DefaultCurrencyCode    = dto?.DefaultCurrency ?? "AUD",
            };

            _context.TravelPolicies.Add(defaultPolicy);

            newClient.DefaultTravelPolicyId = defaultPolicy.Id;
            newClient.DefaultTravelPolicy   = defaultPolicy;

            await _context.SaveChangesAsync();

            await _loggerService.LogInfoAsync($"Default TravelPolicy created with Id: '{defaultPolicy.Id}' for AvaClientId: '{newClient.Id}'.");
            await _loggerService.LogDebugAsync($"Updated AvaClientId: '{newClient.Id}' with DefaultTravelPolicyId '{defaultPolicy.Id}'.");
            
            return Ok();
        }
        else
        {
            await _loggerService.LogInfoAsync($"Updating existing AvaClient with Id: {existingClient.Id}, ClientId: {existingClient.ClientId}");
            existingClient.CompanyName = dto.CompanyName;
            existingClient.TaxIdType = dto?.TaxIdType ?? null;
            existingClient.TaxId = dto?.TaxId ?? null;
            existingClient.TaxLastValidated = dto?.TaxLastValidated ?? null;
            existingClient.LastUpdated = DateTime.UtcNow;

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

            existingClient.DefaultTravelPolicyId = dto?.DefaultTravelPolicyId is { Length: > 0 }
                    ? dto?.DefaultTravelPolicyId
                    : null;
            existingClient.LicenseAgreementId = dto?.LicenseAgreementId is { Length: > 0 }
                    ? dto?.LicenseAgreementId
                    : null;

            if (!string.IsNullOrEmpty(dto?.Country) && !string.IsNullOrEmpty(dto?.TaxId))
            {
                existingClient.TaxLastValidated = await _taxValidation.ValidateTaxRegistrationAsync(dto.TaxId, dto.Country);
                await _loggerService.LogDebugAsync($"Tax validation result for update {dto.TaxId}, {dto.Country}: {existingClient.TaxLastValidated}");
            }

            await _context.SaveChangesAsync();
            await _loggerService.LogInfoAsync($"AvaClient updated with Id: {existingClient.Id}");
            return Ok();
        }
    }

    // V1: Search everything for record match
    [HttpPost("~/api/v1/avaclient/search-everything/{searchValue}")]
    public async Task<IActionResult> SearchEverythingV1(string searchValue)
    {
        // Validate token
        var (isValid, errorResult) = await ValidateBearerTokenAsync();
        if (!isValid)
        {
            await _loggerService.LogWarningAsync(
                $"Unauthorized call to SearchEverythingV1 for '{searchValue}'."
            );
            return errorResult!;
        }

        // Entering and input debug
        await _loggerService.LogTraceAsync("Entering SearchEverythingV1");
        await _loggerService.LogDebugAsync($"SearchEverythingV1 called with searchValue={searchValue}");

        // Lookup
        var client = await _context.AvaClients
            .FirstOrDefaultAsync(c =>
                c.ClientId              == searchValue ||
                c.CompanyName           == searchValue ||
                c.TaxId                 == searchValue ||
                c.AdminPersonEmail      == searchValue ||
                c.BillingPersonEmail    == searchValue ||
                c.ContactPersonEmail    == searchValue ||
                c.AdminPersonPhone      == searchValue ||
                c.BillingPersonPhone    == searchValue ||
                c.ContactPersonPhone    == searchValue ||
                c.DefaultTravelPolicyId == searchValue ||
                c.LicenseAgreementId    == searchValue
            );

        if (client is null)
        {
            await _loggerService.LogInfoAsync(
                $"SearchEverythingV1: no client found matching “{searchValue}”"
            );
            return NotFound($"No client found matching '{searchValue}'.");
        }

        await _loggerService.LogInfoAsync(
            $"SearchEverythingV1: found client with ClientId={client.ClientId}"
        );
        return Ok(new { clientId = client.ClientId });
    }

    // --- LicenseAgreement endpoints ---
    [HttpGet("~/api/v1/avaclient/license/byid/{id}")]
    public async Task<ActionResult<LicenseAgreement>> GetLicenseAgreementById(string id)
    {
        var ag = await _licenseService.GetByIdAsync(id);
        if (ag is null) return NotFound($"LicenseAgreement '{id}' not found.");
        return Ok(ag);
    }

    [HttpPost("~/api/v1/avaclient/license")]
    public async Task<ActionResult<LicenseAgreement>> CreateLicenseAgreement([FromBody] LicenseAgreement agreement)
    {
        var created = await _licenseService.CreateAsync(agreement);
        return CreatedAtAction(
            nameof(GetLicenseAgreementById),
            new { id = created.Id },
            created
        );
    }

    [HttpPut("~/api/v1/avaclient/license/byid/{id}")]
    public async Task<IActionResult> UpdateLicenseAgreement(string id, [FromBody] LicenseAgreement updated)
    {
        if (id != updated.Id) return BadRequest("Route-id and body-id must match.");
        if (!await _licenseService.ExistsAsync(id))
            return NotFound($"LicenseAgreement '{id}' not found.");

        await _licenseService.UpdateAsync(updated);
        return NoContent();
    }

    [HttpDelete("~/api/v1/avaclient/license/byid/{id}")]
    public async Task<IActionResult> DeleteLicenseAgreement(string id)
    {
        if (!await _licenseService.ExistsAsync(id))
            return NotFound($"LicenseAgreement '{id}' not found.");

        await _licenseService.DeleteAsync(id);
        return NoContent();
    }

    // --- LateFeeConfig endpoints (nested under LicenseAgreement) ---
    [HttpGet("~/api/v1/avaclient/license/byid/{licenseAgreementId}/latefeeconfigs")]
    public async Task<ActionResult<IEnumerable<LateFeeConfig>>> GetLateFeeConfigsForAgreement(string licenseAgreementId)
    {
        if (!await _licenseService.ExistsAsync(licenseAgreementId))
            return NotFound($"LicenseAgreement '{licenseAgreementId}' not found.");

        var list = await _lateFeeService.GetByAgreementIdAsync(licenseAgreementId);
        return Ok(list);
    }

    [HttpGet("~/api/v1/avaclient/latefeeconfigs/byid/{id}")]
    public async Task<ActionResult<LateFeeConfig>> GetLateFeeConfigById(string id)
    {
        var cfg = await _lateFeeService.GetByIdAsync(id);
        if (cfg is null) return NotFound($"LateFeeConfig '{id}' not found.");
        return Ok(cfg);
    }

    /// <summary>
    /// Generates a LateFeeConfig AFTER the license has been successfully created
    /// </summary>
    /// <param name="licenseAgreementId">The License Agreement Id to which should be updated with the LateFeeConfig</param>
    /// <param name="template">A template to use for the LateFeeConfig</param>
    /// <returns></returns>
    [HttpPost("~/api/v1/avaclient/license/byid/{licenseAgreementId}/latefeeconfigs")]
    public async Task<ActionResult<LateFeeConfig>> CreateLateFeeConfig(
        string licenseAgreementId,
        [FromBody] LateFeeConfig template)
    {
        // template.LicenseAgreementId is ignored; we enforce route-id
        var created = await _lateFeeService.CreateForAgreementAsync(licenseAgreementId, template);
        return CreatedAtAction(
            nameof(GetLateFeeConfigById),
            new { id = created.Id },
            created
        );
    }

    [HttpPost("~/api/v1/avaclient/license/byid/{licenseAgreementId}/quickgen/latefeeconfig")]
    public async Task<IActionResult> QuickGenLateFeeConfigAsync(string licenseAgreementId)
        => Ok(new { lateFeeConfigId = await _lateFeeService
                                                .QuickGenLateFeeConfigAsync(licenseAgreementId) });


    [HttpPut("~/api/v1/avaclient/latefeeconfigs/byid/{id}")]
    public async Task<IActionResult> UpdateLateFeeConfig(string id, [FromBody] LateFeeConfig updated)
    {
        if (id != updated.Id) return BadRequest("Route-id and body-id must match.");
        if (!await _lateFeeService.ExistsAsync(id))
            return NotFound($"LateFeeConfig '{id}' not found.");

        await _lateFeeService.UpdateAsync(updated);
        return NoContent();
    }

    [HttpDelete("~/api/v1/avaclient/latefeeconfigs/byid/{id}")]
    public async Task<IActionResult> DeleteLateFeeConfig(string id)
    {
        if (!await _lateFeeService.ExistsAsync(id))
            return NotFound($"LateFeeConfig '{id}' not found.");

        await _lateFeeService.DeleteAsync(id);
        return NoContent();
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
