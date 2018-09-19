using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class MotoristaMap : IEntityTypeConfiguration<Motorista>
    {
        public void Configure(EntityTypeBuilder<Motorista> builder)
        {
            builder.ToTable("motorista");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdMotorista");
            builder.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<Motorista>(x => x.IdUsuario);

            builder.Property(x => x.Cnh);
        }
    }
}
