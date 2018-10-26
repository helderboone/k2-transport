using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class LogMap : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("log");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdLog");
            builder.Property(x => x.Data);
            builder.Property(x => x.NomeOrigem);
            builder.Property(x => x.Tipo);
            builder.Property(x => x.Mensagem);
            builder.Property(x => x.Usuario);
            builder.Property(x => x.ExceptionInfo);
        }
    }
}
