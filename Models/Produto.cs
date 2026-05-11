namespace CafeteriaAPI.Models;

public class Produto
{
    public int IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public decimal Custo { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; } = true;

    // Chave estrangeira para Categoria
    public int IdCategoria { get; set; }
    public Categoria Categoria { get; set; } = null!;

    // Navegação: um produto aparece em muitos itens de pedido
    public ICollection<ItemPedido> ItensPedido { get; set; } = [];
}
