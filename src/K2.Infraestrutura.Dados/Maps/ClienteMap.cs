using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cliente");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdCliente");
            builder.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<Cliente>(x => x.IdUsuario);

            builder.Property(x => x.Cep);
            builder.Property(x => x.Endereco);
            builder.Property(x => x.Municipio);
            builder.Property(x => x.Uf);
        }
    }
}
