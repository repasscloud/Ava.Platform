namespace Ava.API.Controllers.Kernel;

[ApiController]
[Route("api/v1/auth")]
public class AvaEmployeeAuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAvaEmployeeService _avaEmployeeService;
    private readonly ApplicationDbContext _context;
    private readonly ICustomPasswordHasher _passwordHasher;
    private readonly ILoggerService _loggerService;

    public AvaEmployeeAuthController(
        IJwtTokenService jwtTokenService,
        IAvaEmployeeService avaEmployeeService,
        ApplicationDbContext context,
        ICustomPasswordHasher passwordHasher,
        ILoggerService loggerService)
    {
        _jwtTokenService = jwtTokenService;
        _avaEmployeeService = avaEmployeeService;
        _context = context;
        _passwordHasher = passwordHasher;
        _loggerService = loggerService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AvaEmployeeLoginDTO dto)
    {
        var user = await _avaEmployeeService.GetByIdOrEmailAsync(dto.Username);

        if (user is null)
            return Unauthorized("User not found.");
        
        if (!string.IsNullOrEmpty(user.PasswordHash) && 
            _passwordHasher.VerifyPassword(user.PrivateKey, dto.Password, user.PasswordHash))
        {
            // crate JWT Token now
            var token = await _jwtTokenService.GenerateTokenAsync(
                user.Id,
                user.Email,
                user.Role.ToString(),
                expiryMinutes: 480
            );

            // create an AvaJwtTokenResponse for quick verification
            var savedToken = new AvaJwtTokenResponse()
            {
                Id = 0,
                JwtToken = token,
                Expires = DateTime.UtcNow.AddMinutes(480),
                IsValid = true,
            };

            // save the token to the DB
            var result = await _jwtTokenService.SaveTokenToDbAsync(savedToken);
            if (result)
            {
                return Ok(new AvaEmployeeLoginResponseDTO { Token = token });
            }
            else
            {
                return BadRequest("Token not saved to db.");
            }
        }

        return Unauthorized("Password incorrect.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AvaEmployeeRegisterDTO dto)
    {
        // var token = TokenUtils.GetBearerToken(HttpContext);
        // if (token == null)
        // {
        //     await _loggerService.LogErrorAsync($"Missing Bearer token for creating user with email '{dto.Email}'.");
        //     return BadRequest("Missing Bearer token.");
        // }

        // var existingToken = await _context.AvaJwtTokenResponses
        //     .FirstOrDefaultAsync(x => x.JwtToken == token);

        // if (existingToken == null)
        // {
        //     await _loggerService.LogErrorAsync($"Token not found for creating user with email '{dto.Email}'.");
        //     return BadRequest("Token not found.");
        // }

        // if (!existingToken.IsValid)
        // {
        //     await _loggerService.LogErrorAsync($"Token is marked as invalid for creating user with email '{dto.Email}'.");
        //     return BadRequest("Token is marked as invalid.");
        // }
            
        // if (existingToken.Expires <= DateTime.UtcNow)
        // {
        //     await _loggerService.LogErrorAsync($"Token '{token}' has expired for creating user with email '{dto.Email}'.");
        //     return BadRequest("Token '{token}' has expired for creating user with email '{dto.Email}'.");
        // }

        // create the new user object          
        var newUser = new AvaEmployeeRecord()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email.ToLowerInvariant(),
            EmployeeType = dto.EmployeeType,
            VerificationToken = Nanoid.Generate(size:18)
        };

        await _context.AvaEmployees.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { newUser.VerificationToken });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDTO dto)
        => (await _avaEmployeeService.ResetPasswordAsync(dto.Email)) 
            ? Ok("If an account exists for the provided information, a password reset has been initiated.")
            : Ok("If an account exists for the provided information, a password reset has been initiated.");

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
        => (await _avaEmployeeService.DeleteAsync(id)) ? Ok() : NotFound();

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AvaEmployeeUpdateDTO dto)
        => (await _avaEmployeeService.UpdateAsync(id, dto)) ? Ok() : NotFound();

    [HttpPost("verify-account")]
    public async Task<IActionResult> VerifyAccount([FromBody] AvaEmployeeVerifyAccountDTO dto)
        => (await _avaEmployeeService.VerifyAccountAsync(dto)) ? Ok() : NotFound();
}


//     [HttpPost("impersonate")]
//     public async Task<IActionResult> Impersonate([FromQuery] string role)
//     {
//         var ok = await _userService.ImpersonateAsRoleAsync(role);
//         return ok ? Ok() : BadRequest("Invalid role");
//     }
// }






// // JwtTokenService.cs (add this method)
// public async Task<bool> ValidateTokenAsync(string jwtToken)
// {
//     var handler = new JwtSecurityTokenHandler();
//     var token = handler.ReadJwtToken(jwtToken);
//     var now = DateTime.UtcNow;
//     var exp = token.ValidTo;

//     var isStored = await _context.AvaAIAppJwtTokens
//         .AnyAsync(t => t.JwtToken == jwtToken && t.IsValid && t.Expires > now);

//     return isStored && exp > now;
// } // 【13†JwtTokenService.cs】
