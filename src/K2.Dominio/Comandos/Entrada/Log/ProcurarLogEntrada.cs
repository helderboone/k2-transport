using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Entidades;
using System;
using System.Reflection;

namespace K2.Dominio.Comandos.Entrada
{
    public class ProcurarLogEntrada : ProcurarEntrada
    {
        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string NomeOrigem { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }

        public string Usuario { get; set; }

        public string ExceptionInfo { get; set; }

        public ProcurarLogEntrada(string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                string.IsNullOrEmpty(ordenarPor) ? "Data" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "DESC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Log).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), $"A propriedade {this.OrdenarPor} não pertence a classe \"Log\".");
        }
    }
}
