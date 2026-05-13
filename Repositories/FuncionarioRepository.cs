using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Data;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;

namespace CafeteriaAPI.Repositories;

public class FuncionarioRepository(AppDbContext context) : IFuncionarioRepository
{
    public async Task<IEnumerable<Funcionario>> GetAllAsync() =>
        await context.Funcionarios.OrderBy(f => f.Nome).ToListAsync();

    public async Task<Funcionario?> GetByIdAsync(int id) =>
        await context.Funcionarios.FindAsync(id);

    public async Task<Funcionario> CreateAsync(Funcionario funcionario)
    {
        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();
        return funcionario;
    }

    public async Task<Funcionario?> UpdateAsync(int id, Funcionario atualizado)
    {
        var func = await context.Funcionarios.FindAsync(id);
        if (func is null) return null;
        func.Nome        = atualizado.Nome;
        func.Email       = atualizado.Email;
        func.Cargo       = atualizado.Cargo;
        func.Salario     = atualizado.Salario;
        func.Ativo       = atualizado.Ativo;
        await context.SaveChangesAsync();
        return func;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var func = await context.Funcionarios.FindAsync(id);
        if (func is null) return false;
        func.Ativo = false;
        await context.SaveChangesAsync();
        return true;
    }
}
