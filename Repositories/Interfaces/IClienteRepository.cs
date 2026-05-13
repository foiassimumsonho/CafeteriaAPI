using CafeteriaAPI.Models;

namespace CafeteriaAPI.Repositories.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(int id);
    Task<Cliente> CreateAsync(Cliente cliente);
    Task<Cliente?> UpdateAsync(int id, Cliente cliente);
    Task<bool> DeleteAsync(int id);
}
