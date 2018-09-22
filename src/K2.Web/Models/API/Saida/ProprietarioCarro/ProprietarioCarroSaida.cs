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
        /// <summary>
        /// Indica se o proprietário está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Id do proprietario
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

        public ProprietarioCarroRetorno(
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
