using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Data;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;

namespace CafeteriaAPI.Repositories;

public class PagamentoRepository(AppDbContext context) : IPagamentoRepository
{
    public async Task<IEnumerable<Pagamento>> GetAllAsync() =>
        await context.Pagamentos
            .Include(p => p.Pedido)
                .ThenInclude(p => p.Cliente)
            .OrderByDescending(p => p.DataPagamento)
            .ToListAsync();

    public async Task<Pagamento?> GetByPedidoAsync(int idPedido) =>
        await context.Pagamentos.FirstOrDefaultAsync(p => p.IdPedido == idPedido);

    public async Task<Pagamento> CreateAsync(Pagamento pagamento)
    {
        context.Pagamentos.Add(pagamento);
        await context.SaveChangesAsync();
        return pagamento;
    }
}
