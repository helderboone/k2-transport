using System;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um registro do log
    /// </summary>
    public class Log
    {
        /// <summary>
        /// ID do registro
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Data do registro
        /// </summary>
        public DateTime Data { get; private set; }

        /// <summary>
        /// Nome da origem do registro
        /// </summary>
        public string NomeOrigem { get; private set; }

        /// <summary>
        /// Tipo do registro
        /// </summary>
        public string Tipo { get; private set; }

        /// <summary>
        /// Mensagem do registro
        /// </summary>
        public string Mensagem { get; private set; }

        /// <summary>
        /// Nome do usuário associado ao registro
        /// </summary>
        public string Usuario { get; private set; }

        /// <summary>
        /// Informações relacionadas a exception logada
        /// </summary>
        public string ExceptionInfo { get; private set; }

        private Log()
        {

        }
    }
}
