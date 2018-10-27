using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por registros do log
    /// </summary>
    public class ProcurarLogEntrada : ProcurarEntrada
    {
        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string NomeOrigem { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }

        public string Usuario { get; set; }

        public string ExceptionInfo { get; set; }
    }
}
