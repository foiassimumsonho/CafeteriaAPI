namespace CafeteriaAPI.Models;

public enum StatusPedido
{
    Aberto,
    EmPreparo,
    Pronto,
    Entregue,
    Cancelado
}

public class Pedido
{
    public int IdPedido { get; set; }
    public int? IdCliente { get; set; }         // nullable = cliente avulso
    public int IdFuncionario { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public StatusPedido Status { get; set; } = StatusPedido.Aberto;
    public string? Observacoes { get; set; }

    // Navegação
    public Cliente? Cliente { get; set; }
    public Funcionario Funcionario { get; set; } = null!;
    public ICollection<ItemPedido> Itens { get; set; } = [];
    public Pagamento? Pagamento { get; set; }
}
