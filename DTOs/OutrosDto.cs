namespace CafeteriaAPI.DTOs;

// ── CLIENTE ────────────────────────────────────────────────
public class ClienteResponseDto
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cpf { get; set; }
    public DateOnly DataCadastro { get; set; }
    public bool Ativo { get; set; }
}

public class ClienteCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cpf { get; set; }
}

// ── PEDIDO ─────────────────────────────────────────────────
public class ItemPedidoCreateDto
{
    public int IdProduto { get; set; }
    public int Quantidade { get; set; }
    public string? Observacao { get; set; }
}

public class PedidoCreateDto
{
    public int? IdCliente { get; set; }
    public int IdFuncionario { get; set; }
    public string? Observacoes { get; set; }
    public List<ItemPedidoCreateDto> Itens { get; set; } = [];
}

public class ItemPedidoResponseDto
{
    public int IdItem { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class PedidoResponseDto
{
    public int IdPedido { get; set; }
    public string? Cliente { get; set; }
    public string Atendente { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemPedidoResponseDto> Itens { get; set; } = [];
}

// ── AUTENTICAÇÃO JWT ───────────────────────────────────────
public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
}

public class FuncionarioCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public DateOnly DataAdmissao { get; set; }
}

// ── RESPOSTA PADRÃO DA API ─────────────────────────────────
// Todas as respostas da API seguem este formato padronizado
public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public string? Mensagem { get; set; }
    public T? Dados { get; set; }

    public static ApiResponse<T> Ok(T dados, string? mensagem = null) =>
        new() { Sucesso = true, Dados = dados, Mensagem = mensagem };

    public static ApiResponse<T> Erro(string mensagem) =>
        new() { Sucesso = false, Mensagem = mensagem };
}
