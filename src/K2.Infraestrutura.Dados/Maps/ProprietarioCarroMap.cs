using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class ProprietarioCarroMap : IEntityTypeConfiguration<ProprietarioCarro>
    {
        public void Configure(EntityTypeBuilder<ProprietarioCarro> builder)
        {
            builder.ToTable("proprietario");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdProprietario");
            builder.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<ProprietarioCarro>(x => x.IdUsuario);
        }
    }
}
