using CafeteriaAPI.DTOs;

namespace CafeteriaAPI.Services.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDto>> GetAllAsync(bool apenasAtivos = true);
    Task<ProdutoResponseDto?> GetByIdAsync(int id);
    Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto);
    Task<ProdutoResponseDto?> UpdateAsync(int id, ProdutoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
