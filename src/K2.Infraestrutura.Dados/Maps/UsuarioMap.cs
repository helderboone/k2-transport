using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdUsuario");
            builder.Property(x => x.Nome);
            builder.Property(x => x.Senha);
            builder.Property(x => x.Email);
            builder.Property(x => x.Cpf);
            builder.Property(x => x.Rg);
            builder.Property(x => x.Celular);
            builder.Property(x => x.Ativo);
            //builder.Property(x => x.Administrador);

            builder.Ignore(x => x.PermissoesAcesso);
        }
    }
}
