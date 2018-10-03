using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Entidades;
using System.Reflection;

namespace K2.Dominio.Comandos.Entrada
{
    public class ProcurarReservaEntrada : ProcurarEntrada
    {
        public int? IdViagem { get; set; }

        public int? IdCliente { get; set; }

        public decimal? ValorPago { get; set; }

        public string Observacao { get; set; }

        public ProcurarReservaEntrada(string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                string.IsNullOrEmpty(ordenarPor) ? "Id" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Reserva).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), $"A propriedade {this.OrdenarPor} não pertence a classe \"Reserva\".");
        }
    }
}
