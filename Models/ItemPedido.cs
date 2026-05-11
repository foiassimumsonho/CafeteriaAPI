namespace CafeteriaAPI.Models;

public class ItemPedido
{
    public int IdItem { get; set; }
    public int IdPedido { get; set; }
    public int IdProduto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string? Observacao { get; set; }

    // Propriedade calculada (não vai para o banco)
    public decimal Subtotal => Quantidade * PrecoUnitario;

    public Pedido Pedido { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
}
