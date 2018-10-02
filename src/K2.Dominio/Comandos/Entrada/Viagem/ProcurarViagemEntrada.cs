using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Entidades;
using System;
using System.Reflection;

namespace K2.Dominio.Comandos.Entrada
{
    public class ProcurarViagemEntrada : ProcurarEntrada
    {
        public int? IdCarro { get; set; }

        public int? IdMotorista { get; set; }

        public int? IdLocalidadeEmbarque { get; set; }

        public int? IdLocalidadeDesembarque { get; set; }

        public string Descricao { get; set; }

        public decimal? ValorPassagem { get; set; }

        public DateTime? DataSaidaInicio { get; set; }

        public DateTime? DataSaidaFim { get; set; }

        public ProcurarViagemEntrada(string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                string.IsNullOrEmpty(ordenarPor) ? "Descricao" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Viagem).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), $"A propriedade {this.OrdenarPor} não pertence a classe \"Viagem\".");

            this.NotificarSeVerdadeiro(this.DataSaidaInicio.HasValue && !this.DataSaidaFim.HasValue || !this.DataSaidaInicio.HasValue && this.DataSaidaFim.HasValue, "O período informado para procurar pela data de saída é inválido. Informe a data início e fim.");

            if (this.DataSaidaInicio.HasValue && this.DataSaidaFim.HasValue)
                this.NotificarSeVerdadeiro(this.DataSaidaInicio > this.DataSaidaFim, "O período informado para procurar pela data de saída é inválido. Verifique as datas informadas.");
        }
    }
}
