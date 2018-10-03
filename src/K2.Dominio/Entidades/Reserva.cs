using K2.Dominio.Comandos.Entrada;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma reserva
    /// </summary>
    public class Reserva
    {
        /// <summary>
        /// ID da reserva
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID da viagem
        /// </summary>
        public int IdViagem { get; private set; }

        /// <summary>
        /// ID do cliente
        /// </summary>
        public int IdCliente { get; private set; }

        /// <summary>
        /// Situação da reserva
        /// </summary>
        public int Situacao { get; private set; }

        /// <summary>
        /// Valor pago da reserva
        /// </summary>
        public decimal? ValorPago { get; private set; }

        /// <summary>
        /// Observações da reserva
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Cliente responsável pela reserva
        /// </summary>
        public Cliente Cliente { get; private set; }

        /// <summary>
        /// Viagem para qual a reserva foi feita
        /// </summary>
        public Viagem Viagem { get; private set; }

        /// <summary>
        /// Dependente da reserva
        /// </summary>
        public ReservaDependente Dependente { get; private set; }

        private Reserva()
        {

        }

        public Reserva(CadastrarReservaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdViagem = entrada.IdViagem;
            this.IdCliente = entrada.IdCliente;
            this.ValorPago = entrada.ValorPago;
            this.Observacao = entrada.Observacao;

            this.Situacao = (int)TipoSituacaoReserva.AguardandoConfirmacao;
        }

        public void Alterar(AlterarReservaEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.IdCliente  = entrada.IdCliente;
            this.ValorPago  = entrada.ValorPago;
            this.Observacao = entrada.Observacao;
        }

        public TipoSituacaoReserva ObterTipoSituacao()
        {
            return JNogueira.Infraestrutura.Utilzao.ExtensionMethods.ConverterParaEnum(this.Situacao, TipoSituacaoReserva.AguardandoConfirmacao);
        }
    }
}
