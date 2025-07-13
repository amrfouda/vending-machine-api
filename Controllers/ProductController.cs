using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly VendingDbContext _context;

    public ProductController(VendingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Products.ToListAsync());
    }

    [Authorize(Roles = "seller")]
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        var username = User.Identity?.Name;
        var seller = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (seller == null) return Unauthorized();
        product.SellerId = seller.Id;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }

    [Authorize(Roles = "seller")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        var username = User.Identity?.Name;
        var seller = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        var existing = await _context.Products.FindAsync(id);
        if (existing == null || existing.SellerId != seller?.Id) return Forbid();
        existing.ProductName = product.ProductName;
        existing.Cost = product.Cost;
        existing.AmountAvailable = product.AmountAvailable;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [Authorize(Roles = "seller")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var username = User.Identity?.Name;
        var seller = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.SellerId != seller?.Id) return Forbid();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok();
    }
}