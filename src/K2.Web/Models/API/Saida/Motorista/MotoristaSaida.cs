using JNogueira.Infraestrutura.Utilzao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um motorista
    /// </summary>
    public class MotoristaSaida : Saida<MotoristaRegistro>
    {
        public MotoristaSaida(bool sucesso, IEnumerable<string> mensagens, MotoristaRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static MotoristaSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<MotoristaSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class MotoristaRegistro
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

        public string Cnh { get; set; }
    }
}
