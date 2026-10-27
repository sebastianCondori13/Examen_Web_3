using Microsoft.EntityFrameworkCore;
using SportHub.Api.Models;
namespace SportHub.Api.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<AppUser>().HasIndex(x => x.Email).IsUnique();
        m.Entity<Sale>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        m.Entity<SaleItem>().HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        m.Entity<SaleItem>().HasOne(x => x.Sale).WithMany(x => x.Items).HasForeignKey(x => x.SaleId).OnDelete(DeleteBehavior.Cascade);
        m.Entity<SaleItem>().HasIndex(x => new { x.SaleId, x.ProductId }).IsUnique();
        m.Entity<Product>().Property(x => x.Price).HasPrecision(12, 2);
        m.Entity<SaleItem>().Property(x => x.UnitPrice).HasPrecision(12, 2);
        m.Entity<Product>().ToTable(t => { t.HasCheckConstraint("CK_Product_Stock", "Stock >= 0"); t.HasCheckConstraint("CK_Product_Price", "CAST(Price AS NUMERIC) > 0"); });
        m.Entity<SaleItem>().ToTable(t => { t.HasCheckConstraint("CK_Item_Quantity", "Quantity > 0"); t.HasCheckConstraint("CK_Item_Price", "CAST(UnitPrice AS NUMERIC) > 0"); });
    }
}
