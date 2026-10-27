using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SportHub.Api.Models;
namespace SportHub.Api.Services;
public class TokenService(IConfiguration config)
{
    public object Create(AppUser user)
    {
        var expires = DateTime.UtcNow.AddHours(1);
        Claim[] claims = [new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Name, user.Name), new(ClaimTypes.Email, user.Email), new(ClaimTypes.Role, user.Role)];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"], claims, expires: expires, signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new {token = new JwtSecurityTokenHandler().WriteToken(token), expiresAt = expires, user = new { user.Id, user.Name, user.Email, user.Role }};
    }
}
