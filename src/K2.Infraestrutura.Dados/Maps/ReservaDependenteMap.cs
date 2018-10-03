using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class ReservaDependenteMap : IEntityTypeConfiguration<ReservaDependente>
    {
        public void Configure(EntityTypeBuilder<ReservaDependente> builder)
        {
            builder.ToTable("reserva_dependente");
            builder.HasKey(x => x.IdReserva);
            builder.Property(x => x.IdReserva).HasColumnName("IdReserva");

            builder.Property(x => x.Nome);
            builder.Property(x => x.DataNascimento);
            builder.Property(x => x.Cpf);
            builder.Property(x => x.Rg);
        }
    }
}
