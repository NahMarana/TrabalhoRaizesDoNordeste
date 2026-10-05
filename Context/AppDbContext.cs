using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Context
{
    public class AppDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=RaizesDoNordeste.db");
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UnidadesEstabelecimento> UnidadesEstabelecimento { get; set; }
        public DbSet<PromocoesCampanhas> PromocoesCampanha { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<PontosFidelidade> PontosFidelidade { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<ItensPedido> ItensPedido { get; set; }
        public DbSet<Fidelidade> Fidelidades { get; set; }
        public DbSet<EstoqueUnidade> EstoquesUnidade { get; set; }
        public DbSet<EstoqueMovimentacao> EstoquesMovimentacao { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.CPF).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<UnidadesEstabelecimento>().HasIndex(u => u.CNPJ).IsUnique();

            //Categoria -> Produtos
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(fk => fk.CategoriaId);

            //Usuario -> UnidadeEstabelecimento
            modelBuilder.Entity<UnidadesEstabelecimento>()
                .HasMany(ue => ue.Usuarios)
                .WithOne(u => u.UnidadesEstabelecimento)
                .HasForeignKey(fk => fk.EstabelecimentoId);

            //Usuario -> LogAuditoria
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.LogAuditorias)
                .WithOne(la => la.Usuarios)
                .HasForeignKey(fk => fk.UsuarioId);

            //Usuario -> Fidelidade
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Fidelidade)
                .WithOne(f => f.Usuarios)
                .HasForeignKey<Fidelidade>(fk => fk.UsuarioId);

            //Usuario -> Pedidos
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pedidos)
                .WithOne(p => p.Usuarios)
                .HasForeignKey(fk => fk.UsuarioId);

            //Usuario -> EstoqueMovimentacao
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.EstoquesMovimentacao)
                .WithOne(m => m.UsuarioUsado)
                .HasForeignKey(fk => fk.UsuarioUsadoId);

            //UnidadeEstabelecimento -> Pedidos
            modelBuilder.Entity<UnidadesEstabelecimento>()
                .HasMany(ue => ue.Pedidos)
                .WithOne(p => p.UnidadesEstabelecimento)
                .HasForeignKey(fk => fk.EstabelecimentoId);

            //UnidadeEstabelecimento -> EstoqueUnidade
            modelBuilder.Entity<UnidadesEstabelecimento>()
                .HasMany(ue => ue.EstoqueUnidades)
                .WithOne(eu => eu.UnidadesEstabelecimento)
                .HasForeignKey(fk => fk.EstabelecimentoId);

            //UnidadeEstabelecimento -> PromocoesCampanhas
            modelBuilder.Entity<UnidadesEstabelecimento>()
                .HasMany(ue => ue.PromocoesCampanhas)
                .WithOne(pc => pc.UnidadesEstabelecimento)
                .HasForeignKey(fk => fk.EstabelecimentoId);

            //Produtos -> EstoqueUnidade
            modelBuilder.Entity<Produto>()
                .HasMany(pr => pr.EstoqueUnidades)
                .WithOne(eu => eu.Produtos)
                .HasForeignKey(fk => fk.ProdutoId);

            //Produtos -> ItensPedido
            modelBuilder.Entity<Produto>()
                .HasMany(pr => pr.ItensPedidos)
                .WithOne(i => i.Produtos)
                .HasForeignKey(fk => fk.ProdutoId);

            //Produtos -> PromocoesCampanhas
            modelBuilder.Entity<Produto>()
                .HasMany(pr => pr.PromocoesCampanhas)
                .WithOne(pc => pc.Produtos)
                .HasForeignKey(fk => fk.ProdutoId);

            //Pedidos -> EstoqueMovimentacao
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.EstoquesMovimentacao)
                .WithOne(m => m.Pedidos)
                .HasForeignKey(fk => fk.PedidoId);

            //Pedidos -> ItensPedido
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.ItensPedidos)
                .WithOne(i => i.Pedidos)
                .HasForeignKey(fk => fk.PedidoId);

            //Pedidos -> Pagamentos
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Pagamentos)
                .WithOne(pg => pg.Pedidos)
                .HasForeignKey<Pagamento>(fk => fk.PedidoId);

            //Pedidos -> PontosFidelidade
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.PontosFidelidades)
                .WithOne(pf => pf.Pedidos)
                .HasForeignKey(fk => fk.PedidoId);

            //Fidelidade -> PontosFidelidade
            modelBuilder.Entity<Fidelidade>()
                .HasMany(f => f.PontosFidelidade)
                .WithOne(pf => pf.Fidelidade)
                .HasForeignKey(fk => fk.FidelizacaoId);

            //EstoqueUnidade -> EstoqueMovimentacao
            modelBuilder.Entity<EstoqueUnidade>()
                .HasMany(eu => eu.EstoquesMovimentacao)
                .WithOne(m => m.EstoqueUnidade)
                .HasForeignKey(fk => fk.EstoqueId);
        }

    }
}
