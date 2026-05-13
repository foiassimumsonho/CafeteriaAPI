using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Data;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;

namespace CafeteriaAPI.Repositories;

public class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public async Task<IEnumerable<Cliente>> GetAllAsync() =>
        await context.Clientes.OrderBy(c => c.Nome).ToListAsync();

    public async Task<Cliente?> GetByIdAsync(int id) =>
        await context.Clientes.FindAsync(id);

    public async Task<Cliente> CreateAsync(Cliente cliente)
    {
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();
        return cliente;
    }

    public async Task<Cliente?> UpdateAsync(int id, Cliente atualizado)
    {
        var cliente = await context.Clientes.FindAsync(id);
        if (cliente is null) return null;

        cliente.Nome     = atualizado.Nome;
        cliente.Email    = atualizado.Email;
        cliente.Telefone = atualizado.Telefone;
        cliente.Cpf      = atualizado.Cpf;
        cliente.Ativo    = atualizado.Ativo;

        await context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await context.Clientes.FindAsync(id);
        if (cliente is null) return false;
        cliente.Ativo = false;
        await context.SaveChangesAsync();
        return true;
    }
}
