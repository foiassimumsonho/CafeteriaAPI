// =============================================================
// Helpers/JwtHelper.cs
// Gera o token JWT a partir dos dados do funcionário
// =============================================================
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CafeteriaAPI.Models;

namespace CafeteriaAPI.Helpers;

public static class JwtHelper
{
    public static string GerarToken(Funcionario funcionario, IConfiguration config)
    {
        var jwtSettings  = config.GetSection("JwtSettings");
        var secret       = jwtSettings["Secret"]!;
        var issuer       = jwtSettings["Issuer"];
        var audience     = jwtSettings["Audience"];
        var expHours     = int.Parse(jwtSettings["ExpirationHours"] ?? "8");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,  funcionario.IdFuncionario.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, funcionario.Nome),
            new Claim(JwtRegisteredClaimNames.Email, funcionario.Email ?? ""),
            new Claim("cargo", funcionario.Cargo.ToString()),  // usado nas policies
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:    issuer,
            audience:  audience,
            claims:    claims,
            expires:   DateTime.UtcNow.AddHours(expHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
