using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagamentosController(IPagamentoService pagamentoService) : ControllerBase
{
    /// <summary>Lista todos os pagamentos</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PagamentoResponseDto>>>> GetAll()
    {
        var pagamentos = await pagamentoService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<PagamentoResponseDto>>.Ok(pagamentos));
    }

    /// <summary>Registra pagamento de um pedido</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PagamentoResponseDto>>> Registrar([FromBody] PagamentoCreateDto dto)
    {
        var pagamento = await pagamentoService.RegistrarAsync(dto);
        return Ok(ApiResponse<PagamentoResponseDto>.Ok(pagamento, "Pagamento registrado com sucesso."));
    }
}
