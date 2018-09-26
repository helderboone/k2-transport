using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um cliente
    /// </summary>
    public class ClienteSaida : Saida
    {
        public ClienteSaida(bool sucesso, IEnumerable<string> mensagens, ClienteRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public ClienteRetorno ObterRetorno() => (ClienteRetorno)this.Retorno;

        public new static ClienteSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ClienteSaida>(json)
                : null;
        }
    }

    public class ClienteRetorno
    {
        public bool Ativo { get; set; }

        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }
    }
}
