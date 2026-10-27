using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using SportHub.Api.Contracts;
using SportHub.Api.Data;
using SportHub.Api.Models;
using SportHub.Api.Services;
namespace SportHub.Api.Controllers;
[ApiController, Route("api/auth")]
public class AuthController(AppDbContext db, TokenService tokens) : ControllerBase
{
    [HttpPost("register"), EnableRateLimiting("auth")]
    public async Task<IActionResult> Register(RegisterRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.Name)) return BadRequest(new {message="Escribe tu nombre."});
        string email = r.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x=>x.Email==email)) return Conflict(new {message="El correo ya está registrado."});
        var user = new AppUser {Name=r.Name.Trim(),Email=email}; // El cliente nunca elige el rol.
        user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(user, r.Password);
        db.Users.Add(user); await db.SaveChangesAsync();
        return StatusCode(201, tokens.Create(user));
    }
    [HttpPost("login"), EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginRequest r)
    {
        var user = await db.Users.SingleOrDefaultAsync(x=>x.Email==r.Email.Trim().ToLowerInvariant());
        if (user is null || new PasswordHasher<AppUser>().VerifyHashedPassword(user,user.PasswordHash,r.Password)==PasswordVerificationResult.Failed)
            return Unauthorized(new {message="Correo o contraseña incorrectos."});
        return Ok(tokens.Create(user));
    }
    [Authorize, HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        int id=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var u=await db.Users.FindAsync(id);
        return u is null ? Unauthorized() : Ok(new {u.Id,u.Name,u.Email,u.Role});
    }
}
