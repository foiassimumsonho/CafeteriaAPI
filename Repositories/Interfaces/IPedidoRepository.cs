using CafeteriaAPI.Models;

namespace CafeteriaAPI.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetAllAsync();
    Task<Pedido?> GetByIdAsync(int id);
    Task<Pedido> CreateAsync(Pedido pedido);
    Task<Pedido> UpdateStatusAsync(int id, StatusPedido novoStatus);
    Task<bool> CancelarAsync(int id);
}
