// =============================================================
// Services/PedidoService.cs
// Regras de negócio para pedidos
// =============================================================
using CafeteriaAPI.Data;
using CafeteriaAPI.DTOs;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;
using CafeteriaAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAPI.Services;

public class PedidoService(IPedidoRepository repo, AppDbContext context) : IPedidoService
{
    public async Task<IEnumerable<PedidoResponseDto>> GetAllAsync()
    {
        var pedidos = await repo.GetAllAsync();
        return pedidos.Select(ToDto);
    }

    public async Task<PedidoResponseDto?> GetByIdAsync(int id)
    {
        var pedido = await repo.GetByIdAsync(id);
        return pedido is null ? null : ToDto(pedido);
    }

    public async Task<PedidoResponseDto> CreateAsync(PedidoCreateDto dto)
    {
        if (dto.Itens.Count == 0)
            throw new InvalidOperationException("O pedido deve ter pelo menos um item.");

        // Busca preços atuais dos produtos e valida estoque
        var itens = new List<ItemPedido>();
        foreach (var itemDto in dto.Itens)
        {
            var produto = await context.Produtos.FindAsync(itemDto.IdProduto)
                ?? throw new KeyNotFoundException($"Produto {itemDto.IdProduto} não encontrado.");

            if (!produto.Ativo)
                throw new InvalidOperationException($"Produto '{produto.Nome}' não está disponível.");

            if (produto.Estoque < itemDto.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.Estoque}.");

            itens.Add(new ItemPedido
            {
                IdProduto     = produto.IdProduto,
                Quantidade    = itemDto.Quantidade,
                PrecoUnitario = produto.Preco,     // preço travado no momento da compra
                Observacao    = itemDto.Observacao
            });

            // Baixa de estoque
            produto.Estoque -= itemDto.Quantidade;
        }

        var pedido = new Pedido
        {
            IdCliente     = dto.IdCliente,
            IdFuncionario = dto.IdFuncionario,
            Observacoes   = dto.Observacoes,
            Status        = StatusPedido.Aberto,
            Itens         = itens
        };

        var criado = await repo.CreateAsync(pedido);
        await context.SaveChangesAsync();

        return ToDto(await repo.GetByIdAsync(criado.IdPedido) ?? criado);
    }

    public async Task<PedidoResponseDto?> AtualizarStatusAsync(int id, StatusPedido novoStatus)
    {
        var pedido = await repo.GetByIdAsync(id);
        if (pedido is null) return null;

        if (pedido.Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Pedidos cancelados não podem ser alterados.");

        await repo.UpdateStatusAsync(id, novoStatus);
        return ToDto(await repo.GetByIdAsync(id) ?? pedido);
    }

    public Task<bool> CancelarAsync(int id) => repo.CancelarAsync(id);

    private static PedidoResponseDto ToDto(Pedido p) => new()
    {
        IdPedido    = p.IdPedido,
        Cliente     = p.Cliente?.Nome,
        Atendente   = p.Funcionario?.Nome ?? string.Empty,
        DataPedido  = p.DataPedido,
        Status      = p.Status.ToString(),
        Observacoes = p.Observacoes,
        ValorTotal  = p.Itens.Sum(i => i.Quantidade * i.PrecoUnitario),
        Itens       = p.Itens.Select(i => new ItemPedidoResponseDto
        {
            IdItem        = i.IdItem,
            Produto       = i.Produto?.Nome ?? string.Empty,
            Quantidade    = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Subtotal      = i.Subtotal
        }).ToList()
    };
}
