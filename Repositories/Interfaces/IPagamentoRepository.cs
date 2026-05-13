using CafeteriaAPI.Models;

namespace CafeteriaAPI.Repositories.Interfaces;

public interface IPagamentoRepository
{
    Task<IEnumerable<Pagamento>> GetAllAsync();
    Task<Pagamento?> GetByPedidoAsync(int idPedido);
    Task<Pagamento> CreateAsync(Pagamento pagamento);
}
