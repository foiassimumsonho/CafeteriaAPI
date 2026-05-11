namespace CafeteriaAPI.Models;

public enum FormaPagamento
{
    Dinheiro,
    CartaoCredito,
    CartaoDebito,
    Pix
}

public class Pagamento
{
    public int IdPagamento { get; set; }
    public int IdPedido { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime DataPagamento { get; set; } = DateTime.Now;

    public Pedido Pedido { get; set; } = null!;
}
