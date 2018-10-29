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

        public DateTime DataExpedicaoCnh { get; set; }

        public string DataExpedicaoCnhToString => this.DataExpedicaoCnh.ToString("dd/MM/yyyy");

        public DateTime DataValidadeCnh { get; set; }

        public string DataValidadeCnhToString => this.DataValidadeCnh.ToString("dd/MM/yyyy");

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }

        public string EnderecoCompleto
        {
            get
            {
                var enderecoCompleto = new List<string>();

                if (!string.IsNullOrEmpty(this.Endereco))
                    enderecoCompleto.Add(this.Endereco);

                if (!string.IsNullOrEmpty(this.Municipio))
                    enderecoCompleto.Add(this.Municipio);

                if (!string.IsNullOrEmpty(this.Uf))
                    enderecoCompleto.Add(this.Uf);

                if (!string.IsNullOrEmpty(this.Cep))
                    enderecoCompleto.Add("CEP: " + this.Cep);

                return string.Join(", ", enderecoCompleto);
            }
        }
    }
}
