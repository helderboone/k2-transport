using JNogueira.Infraestrutura.Utilzao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um proprietário
    /// </summary>
    public class ProprietarioCarroSaida : Saida<ProprietarioCarroRegistro>
    {
        public ProprietarioCarroSaida(bool sucesso, IEnumerable<string> mensagens, ProprietarioCarroRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static ProprietarioCarroSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ProprietarioCarroSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class ProprietarioCarroRegistro
    {
        public bool Ativo { get; set; }

        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf.FormatarCpf();

        public string Rg { get; set; }

        public string Celular { get; set; }

        public string CelularFormatado => this.Celular.Formatar("(##)######-####");

        public IEnumerable<ProprietarioCarroCarroRegistro> Carros { get; set; }
    }

    public class ProprietarioCarroCarroRegistro
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
