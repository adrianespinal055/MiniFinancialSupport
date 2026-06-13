using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniFinancialSupport.Application.Interfaces;

namespace MiniFinancialSupport.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    // IConfiguration nos llega por DI: así leemos la sección "Jwt" de appsettings.
    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public (string Token, DateTime ExpiresAtUtc) GenerateToken(string email, string role)
    {
        // 1) Leemos la config de JWT desde appsettings.
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var minutes = int.Parse(_config["Jwt:ExpiresMinutes"]!);

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(minutes);

        // 2) Claims = la información que viaja DENTRO del token (quién es y su rol).
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)   // este claim habilita [Authorize(Roles = "...")]
        };

        // 3) La firma: convertimos la Key en bytes y firmamos con HMAC-SHA256.
        //    Esa firma es lo que hace que el token no se pueda falsificar sin la Key.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 4) Construimos el token con emisor, audiencia, claims, expiración y firma.
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        // 5) Lo serializamos a string (eso es lo que viaja por la red).
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expiresAtUtc);
    }
}
