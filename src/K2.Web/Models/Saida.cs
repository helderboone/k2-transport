using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Comando para padronização das saídas do domínio
    /// </summary>
    public class Saida
    {
        /// <summary>
        /// Indica se houve sucesso
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagens retornadas
        /// </summary>
        public IEnumerable<string> Mensagens { get; set; }

        /// <summary>
        /// Objeto retornado
        /// </summary>
        public object Retorno { get; set; }
    }
}
