using CafeteriaAPI.DTOs;

namespace CafeteriaAPI.Services.Interfaces;

public interface IFuncionarioService
{
    Task<IEnumerable<FuncionarioResponseDto>> GetAllAsync();
    Task<FuncionarioResponseDto?> GetByIdAsync(int id);
    Task<FuncionarioResponseDto> CreateAsync(FuncionarioCreateDto dto);
    Task<FuncionarioResponseDto?> UpdateAsync(int id, FuncionarioUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
