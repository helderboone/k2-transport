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
            builder.Property(x => x.DataExpedicaoCnh);
            builder.Property(x => x.DataValidadeCnh);
            builder.Property(x => x.Cep);
            builder.Property(x => x.Endereco);
            builder.Property(x => x.Municipio);
            builder.Property(x => x.Uf);
        }
    }
}
