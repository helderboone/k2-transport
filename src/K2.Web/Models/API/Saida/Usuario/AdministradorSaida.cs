using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um cliente
    /// </summary>
    public class AdministradorSaida : Saida
    {
        public AdministradorSaida(bool sucesso, IEnumerable<string> mensagens, AdministradorRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public AdministradorRetorno ObterRetorno() => (AdministradorRetorno)this.Retorno;

        public new static AdministradorSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<AdministradorSaida>(json)
                : null;
        }
    }

    public class AdministradorRetorno
    {
        public bool Ativo { get; set; }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }
    }
}
