using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar uma localidade
    /// </summary>
    public class AlterarReservaEntrada : Notificavel, IEntrada
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

        public AlterarReservaEntrada(
            int id,
            int idCliente,
            decimal? valorPago,
            string observacao)
        {
            Id         = id;
            IdCliente  = idCliente;
            ValorPago  = valorPago;
            Observacao = observacao;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.Id, 0, ReservaResource.Id_Reserva_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdCliente, 0, ClienteResource.Id_Cliente_Nao_Existe);

            if (this.ValorPago.HasValue)
                this.NotificarSeMenorQue(this.ValorPago.Value, 0, ReservaResource.Valor_Pago_Invalido);
        }
    }
}
