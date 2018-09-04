using K2.Dominio.Interfaces.Comandos;
using System.Collections.Generic;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando para padronização das saídas do domínio
    /// </summary>
    public class Saida : ISaida
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

        public Saida(bool sucesso, IEnumerable<string> mensagens, object retorno)
        {
            this.Sucesso = sucesso;
            this.Mensagens = mensagens;
            this.Retorno = retorno;
        }
    }
}
