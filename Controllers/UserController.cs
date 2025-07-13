using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Vending_Machine.Helpers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly VendingDbContext _context;

    public UserController(VendingDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.PasswordHash))
        return BadRequest("Username and password are required.");

        // Hash the password
        user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { user.Id, user.Username, user.Role });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user == null ? NotFound() : Ok(user);
    }

    [Authorize(Roles = "buyer")]
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] int amount)
    {
        var accepted = new[] { 5, 10, 20, 50, 100 };
        if (!accepted.Contains(amount)) return BadRequest("Invalid coin");
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return NotFound();
        user.Deposit += amount;
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [Authorize(Roles = "buyer")]
    [HttpPost("reset")]
    public async Task<IActionResult> ResetDeposit()
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return NotFound();
        user.Deposit = 0;
        await _context.SaveChangesAsync();
        return Ok(user);
    }
}