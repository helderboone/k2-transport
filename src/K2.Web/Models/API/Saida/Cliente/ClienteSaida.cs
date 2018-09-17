using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um cliente
    /// </summary>
    public class ClienteSaida : Saida
    {
        public ClienteSaida(bool sucesso, IEnumerable<string> mensagens, ClienteRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public ClienteRetorno ObterRetorno() => (ClienteRetorno)this.Retorno;

        public new static ClienteSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ClienteSaida>(json)
                : null;
        }
    }

    /// <summary>
    /// Classe que reflete as informações do token JWT retornado no processo de autenticação
    /// </summary>
    public class ClienteRetorno
    {
        /// <summary>
        /// Indica se o cliente está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Id do cliente
        /// </summary>
        public int Id { get; }

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
        /// CEP do cliente
        /// </summary>
        public string Cep { get; }

        /// <summary>
        /// Descrição do endereço do cliente
        /// </summary>
        public string Endereco { get; }

        /// <summary>
        /// Nome do município do cliente
        /// </summary>
        public string Municipio { get; }

        /// <summary>
        /// Sigla da UF do cliente
        /// </summary>
        public string Uf { get; }

        public ClienteRetorno(
            int id,
            bool ativo,
            string nome,
            string email,
            string cpf,
            string rg,
            string celular,
            string cep,
            string endereco,
            string municipio,
            string uf)
        {
            Ativo     = ativo;
            Id        = id;
            Nome      = nome;
            Email     = email;
            Cpf       = cpf;
            Rg        = rg;
            Celular   = celular;
            Cep       = cep;
            Endereco  = endereco;
            Municipio = municipio;
            Uf        = uf;
        }
    }
}
