using Microsoft.EntityFrameworkCore;
using CafeteriaAPI.Models;

namespace CafeteriaAPI.Data;

// AppDbContext é a "ponte" entre o C# e o banco de dados.
// Cada DbSet<T> corresponde a uma tabela.
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Categoria>   Categorias   => Set<Categoria>();
    public DbSet<Produto>     Produtos     => Set<Produto>();
    public DbSet<Cliente>     Clientes     => Set<Cliente>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Pedido>      Pedidos      => Set<Pedido>();
    public DbSet<ItemPedido>  ItensPedido  => Set<ItemPedido>();
    public DbSet<Pagamento>   Pagamentos   => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── CATEGORIA ──────────────────────────────────────────
        modelBuilder.Entity<Categoria>(e =>
        {
            e.ToTable("categorias");
            e.HasKey(c => c.IdCategoria);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(50);
            e.HasIndex(c => c.Nome).IsUnique();
        });

        // ── PRODUTO ────────────────────────────────────────────
        modelBuilder.Entity<Produto>(e =>
        {
            e.ToTable("produtos");
            e.HasKey(p => p.IdProduto);
            e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            e.Property(p => p.Preco).HasColumnType("decimal(8,2)");
            e.Property(p => p.Custo).HasColumnType("decimal(8,2)");
            e.HasOne(p => p.Categoria)
             .WithMany(c => c.Produtos)
             .HasForeignKey(p => p.IdCategoria)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── CLIENTE ────────────────────────────────────────────
        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("clientes");
            e.HasKey(c => c.IdCliente);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            e.Property(c => c.Email).IsRequired().HasMaxLength(150);
            e.HasIndex(c => c.Email).IsUnique();
            e.HasIndex(c => c.Cpf).IsUnique();
        });

        // ── FUNCIONÁRIO ────────────────────────────────────────
        modelBuilder.Entity<Funcionario>(e =>
        {
            e.ToTable("funcionarios");
            e.HasKey(f => f.IdFuncionario);
            e.Property(f => f.Nome).IsRequired().HasMaxLength(100);
            e.Property(f => f.Cargo).HasConversion<string>(); // salva como texto no banco
            e.Property(f => f.Salario).HasColumnType("decimal(8,2)");
            e.HasIndex(f => f.Email).IsUnique();
        });

        // ── PEDIDO ─────────────────────────────────────────────
        modelBuilder.Entity<Pedido>(e =>
        {
            e.ToTable("pedidos");
            e.HasKey(p => p.IdPedido);
            e.Property(p => p.Status).HasConversion<string>();
            e.HasOne(p => p.Cliente)
             .WithMany(c => c.Pedidos)
             .HasForeignKey(p => p.IdCliente)
             .OnDelete(DeleteBehavior.SetNull);
            e.HasOne(p => p.Funcionario)
             .WithMany(f => f.Pedidos)
             .HasForeignKey(p => p.IdFuncionario)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── ITEM PEDIDO ────────────────────────────────────────
        modelBuilder.Entity<ItemPedido>(e =>
        {
            e.ToTable("itens_pedido");
            e.HasKey(i => i.IdItem);
            e.Property(i => i.PrecoUnitario).HasColumnType("decimal(8,2)");
            e.Ignore(i => i.Subtotal); // não persiste no banco
            e.HasOne(i => i.Pedido)
             .WithMany(p => p.Itens)
             .HasForeignKey(i => i.IdPedido)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.Produto)
             .WithMany(p => p.ItensPedido)
             .HasForeignKey(i => i.IdProduto)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── PAGAMENTO ──────────────────────────────────────────
        modelBuilder.Entity<Pagamento>(e =>
        {
            e.ToTable("pagamentos");
            e.HasKey(p => p.IdPagamento);
            e.Property(p => p.FormaPagamento).HasConversion<string>();
            e.Property(p => p.ValorPago).HasColumnType("decimal(8,2)");
            e.HasOne(p => p.Pedido)
             .WithOne(pe => pe.Pagamento)
             .HasForeignKey<Pagamento>(p => p.IdPedido)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
