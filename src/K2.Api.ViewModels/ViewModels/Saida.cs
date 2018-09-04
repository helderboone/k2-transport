﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Api.ViewModels
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

        public Saida(bool sucesso, IEnumerable<string> mensagens, object retorno)
        {
            this.Sucesso   = sucesso;
            this.Mensagens = mensagens;
            this.Retorno   = retorno;
        }

        public static Saida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<Saida>(json)
                : null;
        }
    }
}