using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Entidades;
using System.Reflection;

namespace K2.Dominio.Comandos.Entrada
{
    public class ProcurarCarroEntrada : ProcurarEntrada
    {
        public int? IdProprietario { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public int? QuantidadeLugares { get; set; }

        public string Placa { get; set; }

        public string Renavam { get; set; }

        public string Caracteristicas { get; set; }

        public ProcurarCarroEntrada(
            string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(string.IsNullOrEmpty(ordenarPor) ? "Descricao" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Carro).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), $"A propriedade {this.OrdenarPor} não pertence a classe \"Carro\".");
        }
    }
}
