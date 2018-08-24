using K2.Infraestrutura.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class RegistroLogMap : IEntityTypeConfiguration<RegistroLog>
    {
        public void Configure(EntityTypeBuilder<RegistroLog> builder)
        {
            builder.ToTable("log");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdLog");
            builder.Property(x => x.Tipo);
            builder.Property(x => x.Mensagem);
            builder.Property(x => x.DataCriacao);
        }
    }
}
