// =============================================================
// Models/Categoria.cs
// Representa a tabela 'categorias' do banco de dados
// =============================================================
namespace CafeteriaAPI.Models;

public class Categoria
{
    public int IdCategoria { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;

    // Navegação: uma categoria tem muitos produtos
    public ICollection<Produto> Produtos { get; set; } = [];
}
