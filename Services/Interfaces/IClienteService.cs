using CafeteriaAPI.DTOs;

namespace CafeteriaAPI.Services.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteResponseDto>> GetAllAsync();
    Task<ClienteResponseDto?> GetByIdAsync(int id);
    Task<ClienteResponseDto> CreateAsync(ClienteCreateDto dto);
    Task<ClienteResponseDto?> UpdateAsync(int id, ClienteCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
