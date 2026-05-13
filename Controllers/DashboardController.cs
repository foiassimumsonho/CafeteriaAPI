using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Data;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;

namespace CafeteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(AppDbContext context) : ControllerBase
{
    /// <summary>Resumo do dia</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> Get()
    {
        var hoje = DateTime.Today;
        var amanha = hoje.AddDays(1);

        var pedidosHoje = await context.Pedidos
            .Include(p => p.Itens)
            .Where(p => p.DataPedido >= hoje && p.DataPedido < amanha)
            .ToListAsync();

        var faturamento = pedidosHoje
            .Where(p => p.Status != StatusPedido.Cancelado)
            .Sum(p => p.Itens.Sum(i => i.Quantidade * i.PrecoUnitario));

        var dto = new DashboardDto
        {
            TotalPedidosHoje    = pedidosHoje.Count,
            FaturamentoHoje     = faturamento,
            PedidosAbertos      = pedidosHoje.Count(p => p.Status == StatusPedido.Aberto),
            PedidosEmPreparo    = pedidosHoje.Count(p => p.Status == StatusPedido.EmPreparo),
            PedidosProntos      = pedidosHoje.Count(p => p.Status == StatusPedido.Pronto),
            PedidosEntregues    = pedidosHoje.Count(p => p.Status == StatusPedido.Entregue),
            PedidosCancelados   = pedidosHoje.Count(p => p.Status == StatusPedido.Cancelado),
            TotalClientes       = await context.Clientes.CountAsync(c => c.Ativo),
            TotalProdutos       = await context.Produtos.CountAsync(p => p.Ativo)
        };

        return Ok(ApiResponse<DashboardDto>.Ok(dto));
    }
}
