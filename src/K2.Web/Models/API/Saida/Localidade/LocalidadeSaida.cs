using Newtonsoft.Json;
using System.Collections.Generic;
using JNogueira.Infraestrutura.Utilzao;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um motorista
    /// </summary>
    public class LocalidadeSaida : Saida
    {
        public LocalidadeSaida(bool sucesso, IEnumerable<string> mensagens, LocalidadeRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public LocalidadeRetorno ObterRetorno() => (LocalidadeRetorno)this.Retorno;

        public new static LocalidadeSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<LocalidadeSaida>(json)
                : null;
        }
    }

    public class LocalidadeRetorno
    {
        /// <summary>
        /// Id da localidade
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da localidade
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// UF da localidade
        /// </summary>
        public string Uf { get; }

        public string NomeUf => this.Uf.ObterNomeUfPorSiglaUf();

        public LocalidadeRetorno(
            int id,
            string nome,
            string uf)
        {
            Id   = id;
            Nome = nome;
            Uf   = uf;
        }
    }
}
