using CafeteriaAPI.DTOs;

namespace CafeteriaAPI.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    Task<bool> RegistrarFuncionarioAsync(FuncionarioCreateDto dto);
}
