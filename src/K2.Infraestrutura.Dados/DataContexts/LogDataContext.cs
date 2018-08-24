using K2.Infraestrutura.Dados.Maps;
using K2.Infraestrutura.Logging;
using Microsoft.EntityFrameworkCore;

namespace K2.Infraestrutura.Dados
{
    public class LogDataContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<RegistroLog> Log { get; set; }

        public LogDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamentos para utilização do Entity Framework
            modelBuilder.ApplyConfiguration(new RegistroLogMap());
        }
    }
}
