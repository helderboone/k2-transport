using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class ReservaMap : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("reserva");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdReserva");

            builder.HasOne(x => x.Cliente)
                .WithMany()
                .HasForeignKey(x => x.IdCliente);

            builder.HasOne(x => x.Viagem)
                .WithMany(y => y.Reservas)
                .HasForeignKey(x => x.IdViagem)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Dependente)
                .WithOne()
                .HasForeignKey<ReservaDependente>(x => x.IdReserva)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.IdViagem);
            builder.Property(x => x.ValorPago);
            builder.Property(x => x.Observacao);
        }
    }
}
