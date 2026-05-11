using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Data;
using CafeteriaAPI.Models;
using CafeteriaAPI.Repositories.Interfaces;

namespace CafeteriaAPI.Repositories;

// O repositório é responsável APENAS por falar com o banco.
// Toda lógica de negócio fica no Service.
public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
    public async Task<IEnumerable<Produto>> GetAllAsync(bool apenasAtivos = true)
    {
        var query = context.Produtos
            .Include(p => p.Categoria)  // traz a categoria junto (JOIN)
            .AsQueryable();

        if (apenasAtivos)
            query = query.Where(p => p.Ativo);

        return await query.OrderBy(p => p.Nome).ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(int id) =>
        await context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.IdProduto == id);

    public async Task<Produto> CreateAsync(Produto produto)
    {
        context.Produtos.Add(produto);
        await context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto> UpdateAsync(Produto produto)
    {
        context.Produtos.Update(produto);
        await context.SaveChangesAsync();
        return produto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await context.Produtos.FindAsync(id);
        if (produto is null) return false;

        // Soft delete: marca como inativo em vez de excluir do banco
        produto.Ativo = false;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await context.Produtos.AnyAsync(p => p.IdProduto == id);
}
