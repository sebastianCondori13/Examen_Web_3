namespace SportHub.Api.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "Vendedor";
}
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Color { get; set; } = "#9674ff";
}
public class Sale
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string CustomerName { get; set; } = "";
    public string Notes { get; set; } = "";
    public List<SaleItem> Items { get; set; } = [];
}
// Tabla intermedia: una venta tiene muchos productos y un producto muchas ventas.
public class SaleItem
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
