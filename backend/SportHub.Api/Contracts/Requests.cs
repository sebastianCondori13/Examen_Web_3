using System.ComponentModel.DataAnnotations;
namespace SportHub.Api.Contracts;
public record RegisterRequest(
    [Required, StringLength(80, MinimumLength = 2)] string Name,
    [Required, EmailAddress, StringLength(120)] string Email,
    [Required, StringLength(100, MinimumLength = 8)] string Password);
public record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);
public record ProductRequest(
    [Required, StringLength(100, MinimumLength = 2)] string Name,
    [StringLength(500)] string Description,
    [Range(typeof(decimal), "0.01", "1000000", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)] decimal Price,
    [Range(0, 1000000)] int Stock,
    [Required, StringLength(80, MinimumLength = 2)] string Category,
    [Required, StringLength(80, MinimumLength = 2)] string Brand,
    [Required, RegularExpression("^#[0-9a-fA-F]{6}$")] string Color);
public record SaleLineRequest([Range(1, int.MaxValue)] int ProductId, [Range(1, 10000)] int Quantity);
public record SaleRequest(
    [Required, StringLength(100, MinimumLength = 2)] string CustomerName,
    [StringLength(500)] string Notes,
    [Required, MinLength(1), MaxLength(100)] List<SaleLineRequest> Items);
