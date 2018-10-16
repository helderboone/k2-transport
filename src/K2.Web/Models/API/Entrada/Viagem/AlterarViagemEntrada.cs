using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Resources;
using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Comando utilizado para o alterar uma viagem
    /// </summary>
    public class AlterarViagemEntrada : BaseModel
    {
        public int Id { get; set; }

        public int IdCarro { get; set; }

        public int IdMotorista { get; set; }

        public int IdLocalidadeEmbarque { get; set; }

        public int IdLocalidadeDesembarque { get; set; }

        public string Descricao { get; set; }

        public decimal ValorPassagem { get; set; }

        public DateTime DataHorarioSaida { get; set; }

        public string[] LocaisEmbarque { get; set; }

        public string[] LocaisDesembarque { get; set; }
    }
}
