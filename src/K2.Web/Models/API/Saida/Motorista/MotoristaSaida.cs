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
        /// <summary>
        /// Indica se o motorista está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Id do motorista
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do Usuario
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// RG do usuário
        /// </summary>
        public string Rg { get; }

        /// <summary>
        /// Celular do usuário
        /// </summary>
        public string Celular { get; }

        /// <summary>
        /// CNH do motorista
        /// </summary>
        public string Cnh { get; }

        /// <summary>
        /// Descrição do endereço do motorista
        /// </summary>
        public string Endereco { get; }

        /// <summary>
        /// Nome do município do motorista
        /// </summary>
        public string Municipio { get; }

        /// <summary>
        /// Sigla da UF do motorista
        /// </summary>
        public string Uf { get; }

        public MotoristaRetorno(
            int id,
            bool ativo,
            string nome,
            string email,
            string cpf,
            string rg,
            string celular,
            string cnh)
        {
            Ativo     = ativo;
            Id        = id;
            Nome      = nome;
            Email     = email;
            Cpf       = cpf;
            Rg        = rg;
            Celular   = celular;
            Cnh       = cnh;
        }
    }
}
