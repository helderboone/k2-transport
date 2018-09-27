using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um cliente
    /// </summary>
    public class AdministradorSaida : Saida<AdministradorRegistro>
    {
        public AdministradorSaida(bool sucesso, IEnumerable<string> mensagens, AdministradorRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static AdministradorSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<AdministradorSaida>(json)
                : null;
        }
    }

    public class AdministradorRegistro
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
