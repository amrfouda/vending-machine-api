using Microsoft.EntityFrameworkCore;

public class VendingDbContext : DbContext
{
    public VendingDbContext(DbContextOptions<VendingDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}