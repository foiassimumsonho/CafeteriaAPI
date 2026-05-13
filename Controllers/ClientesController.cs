// =============================================================
// Controllers/ClientesController.cs
// CRUD completo de clientes
// =============================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController(IClienteService clienteService) : ControllerBase
{
    /// <summary>Lista todos os clientes</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ClienteResponseDto>>>> GetAll()
    {
        var clientes = await clienteService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<ClienteResponseDto>>.Ok(clientes));
    }

    /// <summary>Busca cliente por ID</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> GetById(int id)
    {
        var cliente = await clienteService.GetByIdAsync(id);
        if (cliente is null)
            return NotFound(ApiResponse<ClienteResponseDto>.Erro($"Cliente {id} não encontrado."));
        return Ok(ApiResponse<ClienteResponseDto>.Ok(cliente));
    }

    /// <summary>Cadastra novo cliente</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> Create([FromBody] ClienteCreateDto dto)
    {
        var cliente = await clienteService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = cliente.IdCliente },
            ApiResponse<ClienteResponseDto>.Ok(cliente, "Cliente cadastrado com sucesso."));
    }

    /// <summary>Atualiza dados do cliente</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<ClienteResponseDto>>> Update(int id, [FromBody] ClienteCreateDto dto)
    {
        var cliente = await clienteService.UpdateAsync(id, dto);
        if (cliente is null)
            return NotFound(ApiResponse<ClienteResponseDto>.Erro($"Cliente {id} não encontrado."));
        return Ok(ApiResponse<ClienteResponseDto>.Ok(cliente, "Cliente atualizado com sucesso."));
    }

    /// <summary>Desativa cliente</summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var removido = await clienteService.DeleteAsync(id);
        if (!removido)
            return NotFound(ApiResponse<object>.Erro($"Cliente {id} não encontrado."));
        return Ok(ApiResponse<object>.Ok(null!, "Cliente desativado com sucesso."));
    }
}
