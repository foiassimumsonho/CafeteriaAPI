// =============================================================
// Services/AuthService.cs
// Lida com login e geração de token JWT
// =============================================================
using CafeteriaAPI.Data;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Helpers;
using CafeteriaAPI.Models;
using CafeteriaAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAPI.Services;

public class AuthService(AppDbContext context, IConfiguration config) : IAuthService
{
    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        var funcionario = await context.Funcionarios
            .FirstOrDefaultAsync(f => f.Email == dto.Email && f.Ativo);

        if (funcionario is null) return null;

        // Verifica se a senha bate com o hash salvo
        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(dto.Senha, funcionario.SenhaHash);
        if (!senhaCorreta) return null;

        var token = JwtHelper.GerarToken(funcionario, config);
        var expHours = int.Parse(config["JwtSettings:ExpirationHours"] ?? "8");

        return new LoginResponseDto
        {
            Token     = token,
            Nome      = funcionario.Nome,
            Cargo     = funcionario.Cargo.ToString(),
            Expiracao = DateTime.UtcNow.AddHours(expHours)
        };
    }

    public async Task<bool> RegistrarFuncionarioAsync(FuncionarioCreateDto dto)
    {
        // Verifica se e-mail já existe
        bool emailExiste = await context.Funcionarios
            .AnyAsync(f => f.Email == dto.Email);

        if (emailExiste)
            throw new InvalidOperationException("Já existe um funcionário com este e-mail.");

        // Valida cargo
        if (!Enum.TryParse<Cargo>(dto.Cargo, ignoreCase: true, out var cargo))
            throw new InvalidOperationException($"Cargo inválido: {dto.Cargo}.");

        var funcionario = new Funcionario
        {
            Nome         = dto.Nome.Trim(),
            Email        = dto.Email.ToLower().Trim(),
            SenhaHash    = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Cargo        = cargo,
            Salario      = dto.Salario,
            DataAdmissao = dto.DataAdmissao
        };

        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();
        return true;
    }
}
