using K2.Dominio.Entidades;
using K2.Infraestrutura.Dados.Maps;
using Microsoft.EntityFrameworkCore;

namespace K2.Infraestrutura.Dados
{
    public class EfDataContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Usuario> Usuarios { get; set; }
        //public DbSet<Conta> Contas { get; set; }
        //public DbSet<Periodo> Periodos { get; set; }
        //public DbSet<Pessoa> Pessoas { get; set; }
        //public DbSet<Categoria> Categorias { get; set; }
        //public DbSet<CartaoCredito> CartoesCredito { get; set; }
        //public DbSet<Agendamento> Agendamentos { get; set; }
        //public DbSet<Parcela> Parcelas { get; set; }
        //public DbSet<Lancamento> Lancamentos { get; set; }
        //public DbSet<Anexo> Anexos { get; set; }

        public EfDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamentos para utilização do Entity Framework
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            //modelBuilder.ApplyConfiguration(new ContaMap());
            //modelBuilder.ApplyConfiguration(new PeriodoMap());
            //modelBuilder.ApplyConfiguration(new PessoaMap());
            //modelBuilder.ApplyConfiguration(new CategoriaMap());
            //modelBuilder.ApplyConfiguration(new CartaoCreditoMap());
            //modelBuilder.ApplyConfiguration(new AgendamentoMap());
            //modelBuilder.ApplyConfiguration(new ParcelaMap());
            //modelBuilder.ApplyConfiguration(new LancamentoMap());
            //modelBuilder.ApplyConfiguration(new AnexoMap());
        }
    }
}
