using CafeteriaAPI.Models;

namespace CafeteriaAPI.Repositories.Interfaces;

// Interface define o "contrato": quais operações existem
public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync(bool apenasAtivos = true);
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto> CreateAsync(Produto produto);
    Task<Produto> UpdateAsync(Produto produto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
