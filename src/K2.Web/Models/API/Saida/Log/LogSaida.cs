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

        public LogExceptionRegistro ObterExceptionInfo() => string.IsNullOrEmpty(this.ExceptionInfo) ? null : JsonConvert.DeserializeObject<LogExceptionRegistro>(this.ExceptionInfo);
    }

    public class LogExceptionRegistro
    {
        public LogExceptionInfoRegistro ExceptionInfo { get; set; }

        public LogExceptionRequestRegistro Request { get; set; }
    }

    public class LogExceptionInfoRegistro
    {
        public string ExceptionMensagem { get; set; }

        public string BaseExceptionMensagem { get; set; }

        public string StackTrace { get; set; }

        public string Source { get; set; }
    }

    public class LogExceptionRequestRegistro
    {
        public string Rota { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
