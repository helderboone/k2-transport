using K2.Dominio.Entidades;
using K2.Infraestrutura.Dados.Maps;
using Microsoft.EntityFrameworkCore;

namespace K2.Infraestrutura.Dados
{
    public class EfDataContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<ProprietarioCarro> Proprietarios { get; set; }
        public DbSet<Localidade> Localidades { get; set; }
        public DbSet<Carro> Carros { get; set; }

        public EfDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new MotoristaMap());
            modelBuilder.ApplyConfiguration(new ProprietarioCarroMap());
            modelBuilder.ApplyConfiguration(new LocalidadeMap());
            modelBuilder.ApplyConfiguration(new CarroMap());
        }
    }
}
