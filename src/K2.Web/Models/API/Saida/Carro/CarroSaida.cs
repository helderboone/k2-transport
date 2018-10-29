using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um proprietário
    /// </summary>
    public class CarroSaida : Saida<CarroRegistro>
    {
        public CarroSaida(bool sucesso, IEnumerable<string> mensagens, CarroRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static CarroSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<CarroSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class CarroRegistro
    {
        public int Id { get; set; }

        public int IdProprietario { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public int Capacidade { get; set; }

        public string Placa { get; set; }

        public string Cor { get; set; }

        public string Renavam { get; set; }

        public string NumeroRegistroSeturb { get; set; }

        public string[] Caracteristicas { get; set; }

        public CarroProprietarioCarroRegistro Proprietario { get; set; }
    }

    public class CarroProprietarioCarroRegistro
    {
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }
    }
}
