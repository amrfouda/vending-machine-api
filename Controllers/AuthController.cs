using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vending_Machine.Helpers; // For PasswordHasher

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly VendingDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(VendingDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var hashed = PasswordHasher.Hash(request.Password);
        if (user.PasswordHash != hashed)
            return Unauthorized("Invalid credentials");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var jwtKey = _config["Jwt:Key"];
        var expireHours = int.Parse(_config["Jwt:ExpireHours"] ?? "1");

        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT secret key is missing in configuration.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}