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
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<PontosFidelidade> PontosFidelidade { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<Pagamentos> Pagamentos { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<ItensPedido> ItensPedido { get; set; }
        public DbSet<Fidelidade> Fidelidades { get; set; }
        public DbSet<EstoqueUnidade> EstoquesUnidade { get; set; }
        public DbSet<EstoqueMovimentacao> EstoquesMovimentacao { get; set; }
        public DbSet<Categorias> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.CPF).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<UnidadesEstabelecimento>().HasIndex(u => u.CNPJ).IsUnique();
        }

    }
}
