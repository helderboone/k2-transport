using K2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace K2.Infraestrutura.Dados.Maps
{
    public class ViagemMap : IEntityTypeConfiguration<Viagem>
    {
        public void Configure(EntityTypeBuilder<Viagem> builder)
        {
            builder.ToTable("viagem");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdViagem");

            builder.HasOne(x => x.Carro)
                .WithMany()
                .HasForeignKey(x => x.IdCarro)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Motorista)
               .WithMany()
               .HasForeignKey(x => x.IdMotorista)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LocalidadeEmbarque)
               .WithMany()
               .HasForeignKey(x => x.IdLocalidadeEmbarque)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LocalidadeDesembarque)
               .WithMany()
               .HasForeignKey(x => x.IdLocalidadeDesembarque)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Descricao);
            builder.Property(x => x.ValorPassagem);
            builder.Property(x => x.DataHorarioSaida);
            builder.Property(x => x.Situacao);
            builder.Property(x => x.Embarques);
            builder.Property(x => x.Desembarques);
            builder.Property(x => x.DescricaoCancelamento);

            builder.Ignore(x => x.PercentualDisponibilidade);
            builder.Ignore(x => x.QuantidadeLugaresDisponiveis);
        }
    }
}
