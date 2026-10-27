using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportHub.Api.Models;
namespace SportHub.Api.Data;
public static class SeedData
{
    public static async Task Initialize(AppDbContext db)
    {
        if (await db.Users.AnyAsync()) return;
        var admin = new AppUser { Name = "Administrador SportHub", Email = "admin@sporthub.com", Role = "Admin" };
        var seller = new AppUser { Name = "Vendedor de prueba", Email = "vendedor@sporthub.com" };
        var hasher = new PasswordHasher<AppUser>();
        admin.PasswordHash = hasher.HashPassword(admin, "SportHub2026!");
        seller.PasswordHash = hasher.HashPassword(seller, "SportHub2026!");
        await using var tx=await db.Database.BeginTransactionAsync();
        db.Users.AddRange(admin, seller);
        await db.SaveChangesAsync();
        string[] cats = ["Calzado", "Ropa", "Accesorios"];
        string[] brands = ["Nova", "Apex", "Stride"];
        string[] names = ["Nova Air Runner", "Apex Court Pro", "Stride Urban", "Nova Trail X", "Camiseta Motion", "Short Apex Flex", "Sudadera Stride", "Legging Nova Fit", "Balón Apex Match", "Mochila Stride Go", "Botella Nova Steel", "Guantes Apex Lift"];
        decimal[] prices = [420, 580, 350, 640, 120, 95, 260, 180, 150, 210, 85, 110];
        string[] colors = ["#ad92ff", "#f3a778", "#87d7cc", "#83abed"];
        for (int i=0; i<names.Length; i++) db.Products.Add(new Product {Name=names[i],Description="Equipamiento deportivo para entrenar y disfrutar cada movimiento.",Price=prices[i],Stock=15+i*2,Category=cats[i/4],Brand=brands[i%3],Color=colors[i%4]});
        await db.SaveChangesAsync();
        await tx.CommitAsync();
    }
}
