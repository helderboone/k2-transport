using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um proprietário
    /// </summary>
    public class CarroSaida : Saida
    {
        public CarroSaida(bool sucesso, IEnumerable<string> mensagens, CarroRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public CarroRetorno ObterRetorno() => (CarroRetorno)this.Retorno;

        public new static CarroSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<CarroSaida>(json)
                : null;
        }
    }

    public class CarroRetorno
    {
        public int Id { get; set; }

        public int IdProprietario { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public int QuantidadeLugares { get; set; }

        public string Placa { get; set; }

        public string Renavam { get; set; }

        public string[] Caracteristicas { get; set; }

        public CarroProprietarioCarroRetorno Proprietario { get; set; }
    }

    public class CarroProprietarioCarroRetorno
    {
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }
    }
}
