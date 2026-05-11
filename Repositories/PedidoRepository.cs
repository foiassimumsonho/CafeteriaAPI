// =============================================================
// Repositories/PedidoRepository.cs
// Acesso ao banco para pedidos
// =============================================================
using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Data;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;

namespace CafeteriaAPI.Repositories;

public class PedidoRepository(AppDbContext context) : IPedidoRepository
{
    public async Task<IEnumerable<Pedido>> GetAllAsync() =>
        await context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Funcionario)
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .Include(p => p.Pagamento)
            .OrderByDescending(p => p.DataPedido)
            .ToListAsync();

    public async Task<Pedido?> GetByIdAsync(int id) =>
        await context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Funcionario)
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .Include(p => p.Pagamento)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

    public async Task<Pedido> CreateAsync(Pedido pedido)
    {
        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();
        return pedido;
    }

    public async Task<Pedido> UpdateStatusAsync(int id, StatusPedido novoStatus)
    {
        var pedido = await context.Pedidos.FindAsync(id)
            ?? throw new KeyNotFoundException($"Pedido {id} não encontrado.");

        pedido.Status = novoStatus;
        await context.SaveChangesAsync();
        return pedido;
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var pedido = await context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

        if (pedido is null) return false;
        if (pedido.Status == StatusPedido.Cancelado) return false;

        // Devolve o estoque dos itens cancelados
        foreach (var item in pedido.Itens)
        {
            if (item.Produto is not null)
                item.Produto.Estoque += item.Quantidade;
        }

        pedido.Status = StatusPedido.Cancelado;
        await context.SaveChangesAsync();
        return true;
    }
}
