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
public class RelatorioController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<RelatorioDto>>> Get(
        [FromQuery] int ano = 0,
        [FromQuery] int mes = 0)
    {
        if (ano == 0) ano = DateTime.Today.Year;
        if (mes == 0) mes = DateTime.Today.Month;

        var inicio = new DateTime(ano, mes, 1);
        var fim    = inicio.AddMonths(1);

        var pedidos = await context.Pedidos
            .Include(p => p.Itens)
            .Include(p => p.Cliente)
            .Include(p => p.Pagamento)
            .Where(p => p.DataPedido >= inicio && p.DataPedido < fim)
            .ToListAsync();

        var pedidosNaoCancelados = pedidos.Where(p => p.Status != StatusPedido.Cancelado).ToList();

        var itensPorProduto = await context.ItensPedido
            .Include(i => i.Produto)
            .Include(i => i.Pedido)
            .Where(i => i.Pedido.DataPedido >= inicio && i.Pedido.DataPedido < fim
                     && i.Pedido.Status != StatusPedido.Cancelado)
            .GroupBy(i => i.Produto.Nome)
            .Select(g => new ProdutoRelatorioDto
            {
                Produto      = g.Key,
                Quantidade   = g.Sum(i => i.Quantidade),
                TotalVendido = g.Sum(i => i.Quantidade * i.PrecoUnitario)
            })
            .OrderByDescending(p => p.TotalVendido)
            .ToListAsync();

        var dto = new RelatorioDto
        {
            Ano               = ano,
            Mes               = mes,
            NomeMes           = inicio.ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")),
            TotalPedidos      = pedidos.Count,
            PedidosCancelados = pedidos.Count(p => p.Status == StatusPedido.Cancelado),
            PedidosEntregues  = pedidos.Count(p => p.Status == StatusPedido.Entregue),
            FaturamentoBruto  = pedidosNaoCancelados.Sum(p => p.Itens.Sum(i => i.Quantidade * i.PrecoUnitario)),
            TotalRecebido     = pedidos.Where(p => p.Pagamento != null).Sum(p => p.Pagamento!.ValorPago),
            ProdutosMaisVendidos = itensPorProduto
        };

        return Ok(ApiResponse<RelatorioDto>.Ok(dto));
    }
}
