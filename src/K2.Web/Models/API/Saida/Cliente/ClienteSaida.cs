using JNogueira.Infraestrutura.Utilzao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um cliente
    /// </summary>
    public class ClienteSaida : Saida<ClienteRegistro>
    {
        public ClienteSaida(bool sucesso, IEnumerable<string> mensagens, ClienteRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static ClienteSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ClienteSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class ClienteRegistro
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

        public string Cep { get; set; }

        public string CepFormatado => this.Cep?.Formatar("##.###-###");

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }
    }
}
