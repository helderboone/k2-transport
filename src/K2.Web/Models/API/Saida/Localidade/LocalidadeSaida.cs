using Newtonsoft.Json;
using System.Collections.Generic;
using JNogueira.Infraestrutura.Utilzao;
using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de uma localidade
    /// </summary>
    public class LocalidadeSaida : Saida<LocalidadeRegistro>
    {
        public LocalidadeSaida(bool sucesso, IEnumerable<string> mensagens, LocalidadeRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static LocalidadeSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<LocalidadeSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class LocalidadeRegistro
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Uf { get; set; }

        public string NomeUf => this.Uf.ObterNomeUfPorSiglaUf();
    }
}
