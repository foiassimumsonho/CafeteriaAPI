using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FuncionariosController(IFuncionarioService funcionarioService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<FuncionarioResponseDto>>>> GetAll()
    {
        var funcs = await funcionarioService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<FuncionarioResponseDto>>.Ok(funcs));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<FuncionarioResponseDto>>> GetById(int id)
    {
        var func = await funcionarioService.GetByIdAsync(id);
        if (func is null) return NotFound(ApiResponse<FuncionarioResponseDto>.Erro($"Funcionário {id} não encontrado."));
        return Ok(ApiResponse<FuncionarioResponseDto>.Ok(func));
    }

    [HttpPost]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<FuncionarioResponseDto>>> Create([FromBody] FuncionarioCreateDto dto)
    {
        var func = await funcionarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = func.IdFuncionario },
            ApiResponse<FuncionarioResponseDto>.Ok(func, "Funcionário cadastrado com sucesso."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<FuncionarioResponseDto>>> Update(int id, [FromBody] FuncionarioUpdateDto dto)
    {
        var func = await funcionarioService.UpdateAsync(id, dto);
        if (func is null) return NotFound(ApiResponse<FuncionarioResponseDto>.Erro($"Funcionário {id} não encontrado."));
        return Ok(ApiResponse<FuncionarioResponseDto>.Ok(func, "Funcionário atualizado com sucesso."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var removido = await funcionarioService.DeleteAsync(id);
        if (!removido) return NotFound(ApiResponse<object>.Erro($"Funcionário {id} não encontrado."));
        return Ok(ApiResponse<object>.Ok(null!, "Funcionário desativado com sucesso."));
    }
}
