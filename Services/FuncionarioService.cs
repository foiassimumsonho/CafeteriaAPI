using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Services;

public class FuncionarioService(IFuncionarioRepository repo) : IFuncionarioService
{
    public async Task<IEnumerable<FuncionarioResponseDto>> GetAllAsync()
    {
        var funcs = await repo.GetAllAsync();
        return funcs.Select(ToDto);
    }

    public async Task<FuncionarioResponseDto?> GetByIdAsync(int id)
    {
        var func = await repo.GetByIdAsync(id);
        return func is null ? null : ToDto(func);
    }

    public async Task<FuncionarioResponseDto> CreateAsync(FuncionarioCreateDto dto)
    {
        if (!Enum.TryParse<Cargo>(dto.Cargo, ignoreCase: true, out var cargo))
            throw new InvalidOperationException($"Cargo inválido: {dto.Cargo}.");

        var func = new Funcionario
        {
            Nome         = dto.Nome.Trim(),
            Email        = dto.Email.ToLower().Trim(),
            SenhaHash    = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Cargo        = cargo,
            Salario      = dto.Salario,
            DataAdmissao = dto.DataAdmissao,
            Ativo        = true
        };
        var criado = await repo.CreateAsync(func);
        return ToDto(criado);
    }

    public async Task<FuncionarioResponseDto?> UpdateAsync(int id, FuncionarioUpdateDto dto)
    {
        if (!Enum.TryParse<Cargo>(dto.Cargo, ignoreCase: true, out var cargo))
            throw new InvalidOperationException($"Cargo inválido: {dto.Cargo}.");

        var atualizado = new Funcionario
        {
            Nome    = dto.Nome.Trim(),
            Email   = dto.Email.ToLower().Trim(),
            Cargo   = cargo,
            Salario = dto.Salario,
            Ativo   = true
        };
        var func = await repo.UpdateAsync(id, atualizado);
        return func is null ? null : ToDto(func);
    }

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    private static FuncionarioResponseDto ToDto(Funcionario f) => new()
    {
        IdFuncionario = f.IdFuncionario,
        Nome          = f.Nome,
        Email         = f.Email ?? string.Empty,
        Cargo         = f.Cargo.ToString(),
        Salario       = f.Salario,
        DataAdmissao  = f.DataAdmissao.ToString("dd/MM/yyyy"),
        Ativo         = f.Ativo
    };
}
