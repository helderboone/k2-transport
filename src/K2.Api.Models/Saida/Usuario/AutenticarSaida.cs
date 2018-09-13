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
    public class AutenticarSaida : Saida
    {
        public AutenticarSaida(bool sucesso, IEnumerable<string> mensagens, AutenticarRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        /// <summary>
        /// Obtém o retorno da autenticação
        /// </summary>
        public AutenticarRetorno ObterRetorno() => (AutenticarRetorno)this.Retorno;
        
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

        public new static AutenticarSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<AutenticarSaida>(json)
                : null;
        }
    }

    /// <summary>
    /// Classe que reflete as informações do token JWT retornado no processo de autenticação
    /// </summary>
    public class AutenticarRetorno
    {
        public DateTimeOffset DataCriacaoToken { get; set; }

        public DateTimeOffset DataExpiracaoToken { get; set; }

        public string Token { get; set; }

        public AutenticarRetorno(DateTimeOffset dataCriacaoToken, DateTimeOffset dataExpiracaoToken, string token)
        {
            DataCriacaoToken = dataCriacaoToken;
            DataExpiracaoToken = dataExpiracaoToken;
            Token = token;
        }
    }
}
