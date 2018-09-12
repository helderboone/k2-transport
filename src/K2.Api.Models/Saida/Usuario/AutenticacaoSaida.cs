using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace K2.Api.Models
{
    /// <summary>
    /// Classe que reflete o resultado do processo de autenticação
    /// </summary>
    public class AutenticacaoSaida : Saida
    {
        public AutenticacaoSaida(bool sucesso, IEnumerable<string> mensagens, Retorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public Retorno ObterRetorno() => (Retorno)this.Retorno;

        /// <summary>
        /// Obtem o token JWT
        /// </summary>
        public string ObterToken() => ObterRetorno()?.Token;

        /// <summary>
        /// Extrai o nome do usuário do token JWT
        /// </summary>
        public string ObterNomeUsuario() => this.ObterClaims().FirstOrDefault(x => x.Type == "Nome")?.Value;

        /// <summary>
        /// Extrai os claims do token JWT
        /// </summary>
        public IEnumerable<Claim> ObterClaims()
        {
            var token = ObterToken();

            if (string.IsNullOrEmpty(token))
                return null;

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
                return null;

            var jwtToken = jwtHandler.ReadJwtToken(token);

            return jwtToken.Claims;
        }

        public new static AutenticacaoSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<AutenticacaoSaida>(json)
                : null;
        }
    }

    /// <summary>
    /// Classe que reflete as informações do token JWT retornado no processo de autenticação
    /// </summary>
    public class Retorno
    {
        public DateTimeOffset DataCriacaoToken { get; set; }

        public DateTimeOffset DataExpiracaoToken { get; set; }

        public string Token { get; set; }

        public Retorno(DateTimeOffset dataCriacaoToken, DateTimeOffset dataExpiracaoToken, string token)
        {
            DataCriacaoToken = dataCriacaoToken;
            DataExpiracaoToken = dataExpiracaoToken;
            Token = token;
        }
    }
}
