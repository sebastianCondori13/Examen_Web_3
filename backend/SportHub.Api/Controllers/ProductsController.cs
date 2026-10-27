using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportHub.Api.Contracts;
using SportHub.Api.Data;
using SportHub.Api.Models;
namespace SportHub.Api.Controllers;
[ApiController, Authorize, Route("api/products")]
public class ProductsController(AppDbContext db) : ControllerBase
{
    private IQueryable<object> Rows(int? id=null,string? search=null,string? category=null) => db.Products.AsNoTracking()
        .Where(x=>(id==null||x.Id==id) && (search==null||x.Name.Contains(search)) && (category==null||x.Category==category))
        .OrderBy(x=>x.Id).Select(x=>new {x.Id,x.Name,x.Description,x.Price,x.Stock,x.Color,x.Category,x.Brand});
    [HttpGet] public async Task<IActionResult> List(string? search,string? category) => Ok(await Rows(search:search,category:category).ToListAsync());
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) {var row=await Rows(id).FirstOrDefaultAsync();return row is null?NotFound():Ok(row);}
    private static void Apply(Product p, ProductRequest r) {p.Name=r.Name.Trim();p.Description=r.Description?.Trim()??"";p.Price=decimal.Round(r.Price,2);p.Stock=r.Stock;p.Category=r.Category.Trim();p.Brand=r.Brand.Trim();p.Color=r.Color;}
    [HttpPost, Authorize(Roles="Admin")]
    public async Task<IActionResult> Create(ProductRequest r)
    {
        if(string.IsNullOrWhiteSpace(r.Name)||string.IsNullOrWhiteSpace(r.Category)||string.IsNullOrWhiteSpace(r.Brand)) return BadRequest(new {message="Revisa el nombre, categoría y marca."});
        var p=new Product();Apply(p,r);db.Products.Add(p);await db.SaveChangesAsync();return CreatedAtAction(nameof(Get),new{id=p.Id},await Rows(p.Id).FirstAsync());
    }
    [HttpPut("{id:int}"), Authorize(Roles="Admin")]
    public async Task<IActionResult> Update(int id,ProductRequest r)
    {
        if(string.IsNullOrWhiteSpace(r.Name)||string.IsNullOrWhiteSpace(r.Category)||string.IsNullOrWhiteSpace(r.Brand)) return BadRequest(new {message="Revisa el nombre, categoría y marca."});
        var p=await db.Products.FindAsync(id);if(p is null)return NotFound();Apply(p,r);await db.SaveChangesAsync();return Ok(await Rows(id).FirstAsync());
    }
    [HttpDelete("{id:int}"), Authorize(Roles="Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var p=await db.Products.FindAsync(id);if(p is null)return NotFound();
        if(await db.SaleItems.AnyAsync(x=>x.ProductId==id))return Conflict(new {message="El producto tiene ventas y conserva su historial."});
        db.Remove(p);await db.SaveChangesAsync();return NoContent();
    }
}
