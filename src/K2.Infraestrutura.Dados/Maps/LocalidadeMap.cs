using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class LocalidadeMap : IEntityTypeConfiguration<Localidade>
    {
        public void Configure(EntityTypeBuilder<Localidade> builder)
        {
            builder.ToTable("localidade");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdLocalidade");
            builder.Property(x => x.Nome);
            builder.Property(x => x.Uf);
        }
    }
}
