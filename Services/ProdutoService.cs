using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;
using CafeteriaAPI.Services.Interfaces;

namespace CafeteriaAPI.Services;

// O Service contém as REGRAS DE NEGÓCIO e faz a conversão
// entre DTOs (que a API expõe) e Models (que o banco usa).
public class ProdutoService(IProdutoRepository repo) : IProdutoService
{
    public async Task<IEnumerable<ProdutoResponseDto>> GetAllAsync(bool apenasAtivos = true)
    {
        var produtos = await repo.GetAllAsync(apenasAtivos);
        return produtos.Select(ToDto);
    }

    public async Task<ProdutoResponseDto?> GetByIdAsync(int id)
    {
        var produto = await repo.GetByIdAsync(id);
        return produto is null ? null : ToDto(produto);
    }

    public async Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto)
    {
        // Validação de negócio: preço deve ser maior que custo
        if (dto.Preco <= dto.Custo)
            throw new InvalidOperationException("O preço de venda deve ser maior que o custo.");

        var produto = new Produto
        {
            Nome        = dto.Nome.Trim(),
            Descricao   = dto.Descricao?.Trim(),
            Preco       = dto.Preco,
            Custo       = dto.Custo,
            Estoque     = dto.Estoque,
            IdCategoria = dto.IdCategoria
        };

        var criado = await repo.CreateAsync(produto);
        // Recarrega com a categoria para montar o DTO completo
        return ToDto(await repo.GetByIdAsync(criado.IdProduto) ?? criado);
    }

    public async Task<ProdutoResponseDto?> UpdateAsync(int id, ProdutoUpdateDto dto)
    {
        var produto = await repo.GetByIdAsync(id);
        if (produto is null) return null;

        produto.Nome        = dto.Nome.Trim();
        produto.Descricao   = dto.Descricao?.Trim();
        produto.Preco       = dto.Preco;
        produto.Custo       = dto.Custo;
        produto.Estoque     = dto.Estoque;
        produto.IdCategoria = dto.IdCategoria;
        produto.Ativo       = dto.Ativo;

        await repo.UpdateAsync(produto);
        return ToDto(await repo.GetByIdAsync(id) ?? produto);
    }

    public Task<bool> DeleteAsync(int id) => repo.DeleteAsync(id);

    // Método privado de mapeamento Model → DTO
    private static ProdutoResponseDto ToDto(Produto p) => new()
    {
        IdProduto  = p.IdProduto,
        Nome       = p.Nome,
        Descricao  = p.Descricao,
        Preco      = p.Preco,
        Estoque    = p.Estoque,
        Ativo      = p.Ativo,
        Categoria  = p.Categoria?.Nome ?? string.Empty
    };
}
