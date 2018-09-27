using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete o resultado do processo de autenticação
    /// </summary>
    public class AutenticarSaida : Saida<AutenticarRegistro>
    {
        public AutenticarSaida(bool sucesso, IEnumerable<string> mensagens, AutenticarRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        /// <summary>
        /// Obtem o token JWT
        /// </summary>
        public string ObterToken() => this.Retorno?.Token;

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
                return new List<Claim>();

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
                return new List<Claim>();

            var jwtToken = jwtHandler.ReadJwtToken(token);

            return jwtToken.Claims;
        }

        public static AutenticarSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<AutenticarSaida>(json)
                : null;
        }
    }

    /// <summary>
    /// Classe que reflete as informações do token JWT retornado no processo de autenticação
    /// </summary>
    public class AutenticarRegistro
    {
        public DateTimeOffset DataCriacaoToken { get; }

        public DateTimeOffset DataExpiracaoToken { get; }

        public string Token { get; }

        public AutenticarRegistro(DateTimeOffset dataCriacaoToken, DateTimeOffset dataExpiracaoToken, string token)
        {
            DataCriacaoToken = dataCriacaoToken;
            DataExpiracaoToken = dataExpiracaoToken;
            Token = token;
        }
    }
}
