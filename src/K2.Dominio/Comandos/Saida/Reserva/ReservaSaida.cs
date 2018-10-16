using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa uma reserva
    /// </summary>
    public class ReservaSaida
    {
        /// <summary>
        /// ID da reserva
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// ID do cliente
        /// </summary>
        public int IdCliente { get; }

        /// <summary>
        /// Valor pago da reserva
        /// </summary>
        public decimal? ValorPago { get; }

        /// <summary>
        /// Observações da reserva
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Cliente relacionado a reserva
        /// </summary>
        public object Cliente { get; }

        /// <summary>
        /// Viagem para qual a reserva foi feita
        /// </summary>
        public object Viagem { get; }

        /// <summary>
        /// Dependente da reserva
        /// </summary>
        public ReservaDependenteSaida Dependente { get; }

        public ReservaSaida(Reserva reserva)
        {
            Id                = reserva.Id;
            IdCliente         = reserva.IdCliente;
            ValorPago         = reserva.ValorPago;
            Observacao        = reserva.Observacao;
            Cliente = new
            {
                reserva.Cliente.Nome,
                reserva.Cliente.Rg,
                reserva.Cliente.Cpf,
                reserva.Cliente.Celular
            };
            Viagem = new
            {
                reserva.Viagem.Id,
                reserva.Viagem.Descricao,
                reserva.Viagem.DataHorarioSaida,
                DescricaoSituacao = reserva.Viagem.ObterTipoSituacao().ObterDescricao(),
                reserva.Viagem.ValorPassagem,
                Carro = new
                {
                    reserva.Viagem.Carro.Descricao,
                    reserva.Viagem.Carro.Placa,
                    reserva.Viagem.Carro.QuantidadeLugares
                },
                Motorista = new
                {
                    reserva.Viagem.Motorista.Nome,
                    reserva.Viagem.Motorista.Cnh,
                    reserva.Viagem.Motorista.Cpf,
                    reserva.Viagem.Motorista.Celular
                },
                LocalidadeEmbarque = new
                {
                    reserva.Viagem.LocalidadeEmbarque.Nome,
                    reserva.Viagem.LocalidadeEmbarque.Uf
                },
                LocalidadeDesembarque = new
                {
                    reserva.Viagem.LocalidadeDesembarque.Nome,
                    reserva.Viagem.LocalidadeDesembarque.Uf
                },
                LocaisEmbarque = reserva.Viagem.Embarques?.Split(";".ToCharArray()),
                LocaisDesembarque = reserva.Viagem.Desembarques?.Split(";".ToCharArray()),
            };
            Dependente = reserva.Dependente != null
                ? new ReservaDependenteSaida(reserva.Dependente)
                : null;
        }
    }
}
