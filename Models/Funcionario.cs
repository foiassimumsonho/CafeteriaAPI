namespace CafeteriaAPI.Models;

public enum Cargo
{
    Atendente,
    Barista,
    Caixa,
    Gerente
}

public class Funcionario
{
    public int IdFuncionario { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Cargo Cargo { get; set; }
    public decimal Salario { get; set; }
    public DateOnly DataAdmissao { get; set; }
    public bool Ativo { get; set; } = true;

    // Campos de autenticação (JWT)
    public string? Email { get; set; }
    public string? SenhaHash { get; set; }  // senha criptografada com BCrypt

    public ICollection<Pedido> Pedidos { get; set; } = [];
}
