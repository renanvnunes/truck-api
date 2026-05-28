using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Auth.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string Generate(User user, out TimeSpan expiration)
    {
        var expirationHours = config.GetValue<int>("Jwt:ExpirationHours", 24);
        expiration = TimeSpan.FromHours(expirationHours);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Ulid.NewUlid().ToString()),
            new Claim("role", user.Role.ToString()),
            new Claim("companyId", user.CompanyId ?? string.Empty),
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.Add(expiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
