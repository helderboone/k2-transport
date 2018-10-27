using K2.Dominio.Entidades;
using System;

namespace K2.Dominio.Comandos.Saida
{
    public class LogSaida
    {
        /// <summary>
        /// ID do registro
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Data do registro
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Nome da origem do registro
        /// </summary>
        public string NomeOrigem { get; }

        /// <summary>
        /// Tipo do registro
        /// </summary>
        public string Tipo { get; }

        /// <summary>
        /// Mensagem do registro
        /// </summary>
        public string Mensagem { get; }

        /// <summary>
        /// Nome do usuário associado ao registro
        /// </summary>
        public string Usuario { get; }

        /// <summary>
        /// Informações relacionadas a exception logada
        /// </summary>
        public string ExceptionInfo { get; }

        public LogSaida(Log log)
        {
            this.Id            = log.Id;
            this.Data          = log.Data;
            this.NomeOrigem    = log.NomeOrigem;
            this.Tipo          = log.Tipo;
            this.Mensagem      = log.Mensagem;
            this.Usuario       = log.Usuario;
            this.ExceptionInfo = log.ExceptionInfo;
        }

        public override string ToString()
        {
            return $"{this.Data.ToString("dd/MM/yyyy HH:mm:ss")} - {this.Mensagem}";
        }
    }
}
