// =============================================================
// Controllers/AuthController.cs
// Endpoints de autenticação (login e registro)
// =============================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Autentica um funcionário e retorna o token JWT</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto dto)
    {
        var resultado = await authService.LoginAsync(dto);

        if (resultado is null)
            return Unauthorized(ApiResponse<LoginResponseDto>.Erro("E-mail ou senha inválidos."));

        return Ok(ApiResponse<LoginResponseDto>.Ok(resultado, "Login realizado com sucesso."));
    }

    /// <summary>Registra um novo funcionário (apenas gerentes)</summary>
    [HttpPost("registrar")]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<object>>> Registrar([FromBody] FuncionarioCreateDto dto)
    {
        await authService.RegistrarFuncionarioAsync(dto);
        return Ok(ApiResponse<object>.Ok(null!, "Funcionário registrado com sucesso."));
    }
}
