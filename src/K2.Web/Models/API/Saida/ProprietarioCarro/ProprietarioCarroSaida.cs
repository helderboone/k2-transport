using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um proprietário
    /// </summary>
    public class ProprietarioCarroSaida : Saida
    {
        public ProprietarioCarroSaida(bool sucesso, IEnumerable<string> mensagens, ProprietarioCarroRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public ProprietarioCarroRetorno ObterRetorno() => (ProprietarioCarroRetorno)this.Retorno;

        public new static ProprietarioCarroSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ProprietarioCarroSaida>(json)
                : null;
        }
    }

    public class ProprietarioCarroRetorno
    {
        public bool Ativo { get; set; }

        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }

        public IEnumerable<ProprietarioCarroCarroRetorno> Carros { get; set; }
    }

    public class ProprietarioCarroCarroRetorno
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public int QuantidadeLugares { get; set; }

        public string Placa { get; set; }

        public string Renavam { get; set; }

        public string[] Caracteristicas { get; set; }
    }
}
