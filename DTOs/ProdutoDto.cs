// =============================================================
// DTOs/ProdutoDto.cs
// DTOs são como "formulários": definem o que a API recebe e
// o que ela devolve, sem expor diretamente o Model interno.
// =============================================================
namespace CafeteriaAPI.DTOs;

// O que a API DEVOLVE ao listar/buscar um produto
public class ProdutoResponseDto
{
    public int IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public string Categoria { get; set; } = string.Empty;
}

// O que a API RECEBE ao criar um produto (POST)
public class ProdutoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public decimal Custo { get; set; }
    public int Estoque { get; set; }
    public int IdCategoria { get; set; }
}

// O que a API RECEBE ao editar um produto (PUT)
public class ProdutoUpdateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public decimal Custo { get; set; }
    public int Estoque { get; set; }
    public int IdCategoria { get; set; }
    public bool Ativo { get; set; }
}

// ── CATEGORIA ──────────────────────────────────────────────
public class CategoriaResponseDto
{
    public int IdCategoria { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
}

public class CategoriaCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
