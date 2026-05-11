namespace CafeteriaAPI.Models;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cpf { get; set; }
    public DateOnly DataCadastro { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public bool Ativo { get; set; } = true;

    public ICollection<Pedido> Pedidos { get; set; } = [];
}
