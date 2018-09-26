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
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Uf { get; set; }

        public string NomeUf => this.Uf.ObterNomeUfPorSiglaUf();
    }
}
