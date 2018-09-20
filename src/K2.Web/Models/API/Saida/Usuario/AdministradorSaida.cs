using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um cliente
    /// </summary>
    public class AdministradorSaida : Saida
    {
        public AdministradorSaida(bool sucesso, IEnumerable<string> mensagens, AdministradorRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public AdministradorRetorno ObterRetorno() => (AdministradorRetorno)this.Retorno;

        public new static ClienteSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ClienteSaida>(json)
                : null;
        }
    }

    public class AdministradorRetorno
    {
        /// <summary>
        /// Indica se o cliente está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Id do usuário
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
        

        public AdministradorRetorno(
            int id,
            bool ativo,
            string nome,
            string email,
            string cpf,
            string rg,
            string celular)
        {
            Ativo     = ativo;
            Id        = id;
            Nome      = nome;
            Email     = email;
            Cpf       = cpf;
            Rg        = rg;
            Celular   = celular;
        }
    }
}
