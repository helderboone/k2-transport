using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe para padronização das saídas da API
    /// </summary>
    public class Saida<TRetorno> : BaseModel
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
        public TRetorno Retorno { get; set; }

        public Saida(bool sucesso, IEnumerable<string> mensagens, TRetorno retorno)
        {
            this.Sucesso   = sucesso;
            this.Mensagens = mensagens;
            this.Retorno   = retorno;
        }
    }

    public class Saida : Saida<JObject>
    {
        public Saida(bool sucesso, IEnumerable<string> mensagens, JObject retorno)
            : base(sucesso, mensagens, retorno)
        {

        }

        public static Saida<object> Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<Saida<object>>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }
}
