using CafeteriaAPI.Data;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;
using CafeteriaAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAPI.Services;

public class PagamentoService(IPagamentoRepository repo, AppDbContext context) : IPagamentoService
{
    public async Task<IEnumerable<PagamentoResponseDto>> GetAllAsync()
    {
        var pagamentos = await repo.GetAllAsync();
        return pagamentos.Select(ToDto);
    }

    public async Task<PagamentoResponseDto> RegistrarAsync(PagamentoCreateDto dto)
    {
        // Verifica se pedido existe
        var pedido = await context.Pedidos.FindAsync(dto.IdPedido)
            ?? throw new KeyNotFoundException($"Pedido {dto.IdPedido} não encontrado.");

        // Verifica se já foi pago
        var jaExiste = await repo.GetByPedidoAsync(dto.IdPedido);
        if (jaExiste is not null)
            throw new InvalidOperationException($"Pedido {dto.IdPedido} já possui pagamento registrado.");

        // Valida forma de pagamento
        if (!Enum.TryParse<FormaPagamento>(dto.FormaPagamento, ignoreCase: true, out var forma))
            throw new InvalidOperationException($"Forma de pagamento inválida: {dto.FormaPagamento}.");

        var pagamento = new Pagamento
        {
            IdPedido       = dto.IdPedido,
            FormaPagamento = forma,
            ValorPago      = dto.ValorPago,
            DataPagamento  = DateTime.Now
        };

        // Atualiza status do pedido para Entregue
        pedido.Status = StatusPedido.Entregue;

        var criado = await repo.CreateAsync(pagamento);
        await context.SaveChangesAsync();

        return ToDto(await repo.GetByPedidoAsync(criado.IdPedido) ?? criado);
    }

    private static PagamentoResponseDto ToDto(Pagamento p) => new()
    {
        IdPagamento    = p.IdPagamento,
        IdPedido       = p.IdPedido,
        Cliente        = p.Pedido?.Cliente?.Nome ?? "Avulso",
        FormaPagamento = p.FormaPagamento.ToString(),
        ValorPago      = p.ValorPago,
        DataPagamento  = p.DataPagamento.ToString("dd/MM/yyyy HH:mm")
    };
}
