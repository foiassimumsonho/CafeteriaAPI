// =============================================================
// Controllers/PedidosController.cs
// CRUD e fluxo de status dos pedidos
// =============================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidosController(IPedidoService pedidoService) : ControllerBase
{
    /// <summary>Lista todos os pedidos</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PedidoResponseDto>>>> GetAll()
    {
        var pedidos = await pedidoService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<PedidoResponseDto>>.Ok(pedidos));
    }

    /// <summary>Busca pedido por ID com todos os itens</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PedidoResponseDto>>> GetById(int id)
    {
        var pedido = await pedidoService.GetByIdAsync(id);
        if (pedido is null)
            return NotFound(ApiResponse<PedidoResponseDto>.Erro($"Pedido {id} não encontrado."));

        return Ok(ApiResponse<PedidoResponseDto>.Ok(pedido));
    }

    /// <summary>Abre um novo pedido com seus itens</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PedidoResponseDto>>> Create([FromBody] PedidoCreateDto dto)
    {
        var pedido = await pedidoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = pedido.IdPedido },
            ApiResponse<PedidoResponseDto>.Ok(pedido, "Pedido aberto com sucesso."));
    }

    /// <summary>Atualiza o status do pedido (aberto → em_preparo → pronto → entregue)</summary>
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<ApiResponse<PedidoResponseDto>>> AtualizarStatus(
        int id, [FromQuery] string status)
    {
        if (!Enum.TryParse<StatusPedido>(status, ignoreCase: true, out var novoStatus))
            return BadRequest(ApiResponse<PedidoResponseDto>.Erro($"Status inválido: {status}."));

        var pedido = await pedidoService.AtualizarStatusAsync(id, novoStatus);
        if (pedido is null)
            return NotFound(ApiResponse<PedidoResponseDto>.Erro($"Pedido {id} não encontrado."));

        return Ok(ApiResponse<PedidoResponseDto>.Ok(pedido, "Status atualizado."));
    }

    /// <summary>Cancela o pedido e devolve o estoque</summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Cancelar(int id)
    {
        var cancelado = await pedidoService.CancelarAsync(id);
        if (!cancelado)
            return NotFound(ApiResponse<object>.Erro($"Pedido {id} não encontrado ou já cancelado."));

        return Ok(ApiResponse<object>.Ok(null!, "Pedido cancelado e estoque devolvido."));
    }
}
