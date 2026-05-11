// =============================================================
// Controllers/ProdutosController.cs
// CRUD completo de produtos
// =============================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]   // todas as rotas exigem token (exceto onde [AllowAnonymous])
public class ProdutosController(IProdutoService produtoService) : ControllerBase
{
    /// <summary>Lista todos os produtos ativos</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProdutoResponseDto>>>> GetAll(
        [FromQuery] bool apenasAtivos = true)
    {
        var produtos = await produtoService.GetAllAsync(apenasAtivos);
        return Ok(ApiResponse<IEnumerable<ProdutoResponseDto>>.Ok(produtos));
    }

    /// <summary>Busca produto por ID</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<ProdutoResponseDto>>> GetById(int id)
    {
        var produto = await produtoService.GetByIdAsync(id);
        if (produto is null)
            return NotFound(ApiResponse<ProdutoResponseDto>.Erro($"Produto {id} não encontrado."));

        return Ok(ApiResponse<ProdutoResponseDto>.Ok(produto));
    }

    /// <summary>Cria novo produto (apenas gerentes)</summary>
    [HttpPost]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<ProdutoResponseDto>>> Create([FromBody] ProdutoCreateDto dto)
    {
        var produto = await produtoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = produto.IdProduto },
            ApiResponse<ProdutoResponseDto>.Ok(produto, "Produto criado com sucesso."));
    }

    /// <summary>Atualiza produto (apenas gerentes)</summary>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<ProdutoResponseDto>>> Update(
        int id, [FromBody] ProdutoUpdateDto dto)
    {
        var produto = await produtoService.UpdateAsync(id, dto);
        if (produto is null)
            return NotFound(ApiResponse<ProdutoResponseDto>.Erro($"Produto {id} não encontrado."));

        return Ok(ApiResponse<ProdutoResponseDto>.Ok(produto, "Produto atualizado com sucesso."));
    }

    /// <summary>Desativa produto (soft delete)</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "GerenciadorPolicy")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var removido = await produtoService.DeleteAsync(id);
        if (!removido)
            return NotFound(ApiResponse<object>.Erro($"Produto {id} não encontrado."));

        return Ok(ApiResponse<object>.Ok(null!, "Produto desativado com sucesso."));
    }
}
