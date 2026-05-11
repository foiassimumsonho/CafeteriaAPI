using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;

namespace CafeteriaAPI.Services.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoResponseDto>> GetAllAsync();
    Task<PedidoResponseDto?> GetByIdAsync(int id);
    Task<PedidoResponseDto> CreateAsync(PedidoCreateDto dto);
    Task<PedidoResponseDto?> AtualizarStatusAsync(int id, StatusPedido novoStatus);
    Task<bool> CancelarAsync(int id);
}
