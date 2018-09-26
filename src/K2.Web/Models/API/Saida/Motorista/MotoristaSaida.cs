using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um motorista
    /// </summary>
    public class MotoristaSaida : Saida
    {
        public MotoristaSaida(bool sucesso, IEnumerable<string> mensagens, MotoristaRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public MotoristaRetorno ObterRetorno() => (MotoristaRetorno)this.Retorno;

        public new static MotoristaSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<MotoristaSaida>(json)
                : null;
        }
    }

    public class MotoristaRetorno
    {
        public bool Ativo { get; set; }

        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }

        public string Cnh { get; set; }
    }
}
