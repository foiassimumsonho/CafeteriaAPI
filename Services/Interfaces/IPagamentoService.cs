using CafeteriaAPI.DTOs;

namespace CafeteriaAPI.Services.Interfaces;

public interface IPagamentoService
{
    Task<IEnumerable<PagamentoResponseDto>> GetAllAsync();
    Task<PagamentoResponseDto> RegistrarAsync(PagamentoCreateDto dto);
}
