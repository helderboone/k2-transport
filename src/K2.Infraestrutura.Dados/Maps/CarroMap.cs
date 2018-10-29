using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class CarroMap : IEntityTypeConfiguration<Carro>
    {
        public void Configure(EntityTypeBuilder<Carro> builder)
        {
            builder.ToTable("carro");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdCarro");
            builder.HasOne(x => x.Proprietario)
                .WithMany(y => y.Carros)
                .HasForeignKey(x => x.IdProprietario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Descricao);
            builder.Property(x => x.NomeFabricante);
            builder.Property(x => x.AnoModelo);
            builder.Property(x => x.Capacidade);
            builder.Property(x => x.Placa);
            builder.Property(x => x.Renavam);
            builder.Property(x => x.Caracteristicas);
        }
    }
}
