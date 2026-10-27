using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportHub.Api.Contracts;
using SportHub.Api.Data;
using SportHub.Api.Models;
namespace SportHub.Api.Controllers;
[ApiController, Authorize, Route("api/sales")]
public class SalesController(AppDbContext db) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private IQueryable<Sale> Own => db.Sales.Where(x=>x.UserId==UserId).Include(x=>x.Items).ThenInclude(x=>x.Product);
    private static object View(Sale s)=>new {s.Id,s.CustomerName,s.CreatedAt,s.Notes,total=s.Items.Sum(x=>x.Quantity*x.UnitPrice),items=s.Items.Select(x=>new{x.ProductId,productName=x.Product.Name,x.Quantity,x.UnitPrice,subtotal=x.Quantity*x.UnitPrice})};
    [HttpGet] public async Task<IActionResult> List()=>Ok((await Own.OrderByDescending(x=>x.Id).AsNoTracking().ToListAsync()).Select(View));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id){var s=await Own.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);return s is null?NotFound():Ok(View(s));}
    // La transacción restaura el stock anterior y aplica el nuevo de forma atómica.
    // Los precios se calculan en el servidor, nunca se aceptan desde el cliente.
    private async Task<IActionResult> Save(int? id,SaleRequest r)
    {
        await using var tx=await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        if(string.IsNullOrWhiteSpace(r.CustomerName))return BadRequest(new {message="Escribe el nombre del cliente."});
        if(r.Items is null||r.Items.Count==0||r.Items.Any(x=>x is null||x.Quantity<=0)||r.Items.Select(x=>x.ProductId).Distinct().Count()!=r.Items.Count)
            return BadRequest(new {message="Agrega productos sin repetir y cantidades mayores a cero."});
        Sale s;
        var previousPrices=new Dictionary<int,decimal>();
        if(id.HasValue)
        {
            var old=await Own.FirstOrDefaultAsync(x=>x.Id==id);if(old is null)return NotFound();s=old;
            foreach(var line in s.Items){line.Product.Stock+=line.Quantity;previousPrices[line.ProductId]=line.UnitPrice;}
            db.SaleItems.RemoveRange(s.Items);s.Items.Clear();
            // Libera el índice único antes de insertar las líneas actualizadas.
            await db.SaveChangesAsync();
        }
        else {s=new Sale{UserId=UserId};db.Sales.Add(s);}
        s.CustomerName=r.CustomerName.Trim();s.Notes=r.Notes?.Trim()??"";
        foreach(var line in r.Items)
        {
            var product=await db.Products.FindAsync(line.ProductId);
            if(product is null)return BadRequest(new {message="Uno de los productos ya no existe."});
            if(product.Stock<line.Quantity)return Conflict(new {message=$"Stock insuficiente de {product.Name}. Disponible: {product.Stock}."});
            product.Stock-=line.Quantity;
            s.Items.Add(new SaleItem{Product=product,ProductId=product.Id,Quantity=line.Quantity,UnitPrice=previousPrices.GetValueOrDefault(product.Id,product.Price)});
        }
        await db.SaveChangesAsync();await tx.CommitAsync();
        return id.HasValue?Ok(View(s)):CreatedAtAction(nameof(Get),new{id=s.Id},View(s));
    }
    [HttpPost] public Task<IActionResult> Create(SaleRequest r)=>Save(null,r);
    [HttpPut("{id:int}")] public Task<IActionResult> Update(int id,SaleRequest r)=>Save(id,r);
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id)
    {
        await using var tx=await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var s=await Own.FirstOrDefaultAsync(x=>x.Id==id);if(s is null)return NotFound();
        foreach(var line in s.Items)line.Product.Stock+=line.Quantity;
        db.Sales.Remove(s);await db.SaveChangesAsync();await tx.CommitAsync();return NoContent();
    }
}
