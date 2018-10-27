using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um registro do log
    /// </summary>
    public class LogSaida : Saida<LogRegistro>
    {
        public LogSaida(bool sucesso, IEnumerable<string> mensagens, LogRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static LogSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<LogSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class LogRegistro
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string DataToString => this.Data.ToString("dd/MM/yyyy HH:mm:ss");

        public string NomeOrigem { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }

        public string Usuario { get; set; }

        public string ExceptionInfo { get; set; }
    }
}
