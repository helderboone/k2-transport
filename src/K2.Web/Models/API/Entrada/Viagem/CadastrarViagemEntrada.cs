using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma viagem
    /// </summary>
    public class CadastrarViagemEntrada : BaseModel
    {
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
