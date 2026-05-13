using CafeteriaAPI.Models;

namespace CafeteriaAPI.Repositories.Interfaces;

public interface IFuncionarioRepository
{
    Task<IEnumerable<Funcionario>> GetAllAsync();
    Task<Funcionario?> GetByIdAsync(int id);
    Task<Funcionario> CreateAsync(Funcionario funcionario);
    Task<Funcionario?> UpdateAsync(int id, Funcionario funcionario);
    Task<bool> DeleteAsync(int id);
}
