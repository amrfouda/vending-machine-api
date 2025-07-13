using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/buy")]
public class BuyController : ControllerBase
{
    private readonly VendingDbContext _context;

    public BuyController(VendingDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "buyer")]
    [HttpPost]
    public async Task<IActionResult> Buy([FromBody] BuyRequest request)
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        var product = await _context.Products.FindAsync(request.ProductId);

        if (user == null || product == null) return NotFound();
        if (product.AmountAvailable < request.Quantity) return BadRequest("Not enough product available");

        int totalCost = product.Cost * request.Quantity;
        if (user.Deposit < totalCost) return BadRequest("Insufficient funds");

        user.Deposit -= totalCost;
        product.AmountAvailable -= request.Quantity;
        await _context.SaveChangesAsync();

        var change = CalculateChange(user.Deposit);
        user.Deposit = 0;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            totalSpent = totalCost,
            product.ProductName,
            quantity = request.Quantity,
            change
        });
    }

    private List<int> CalculateChange(int amount)
    {
        var coins = new[] { 100, 50, 20, 10, 5 };
        var result = new List<int>();
        foreach (var coin in coins)
        {
            while (amount >= coin)
            {
                result.Add(coin);
                amount -= coin;
            }
        }
        return result;
    }

    public class BuyRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}