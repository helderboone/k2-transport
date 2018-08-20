using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace K2.Web.Models
{
    public class AutenticacaoSaida
    {
        [JsonProperty("sucesso")]
        public bool Sucesso { get; set; }

        [JsonProperty("mensagens")]
        public string[] Mensagens { get; set; }

        [JsonProperty("retorno")]
        public Retorno Retorno { get; set; }

        public static AutenticacaoSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<AutenticacaoSaida>(json)
                : null;
        }

        public string ObterToken() => this.Retorno?.Token;

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
    }

    public class Retorno
    {
        [JsonProperty("dataCriacaoToken")]
        public DateTimeOffset DataCriacaoToken { get; set; }

        [JsonProperty("dataExpiracaoToken")]
        public DateTimeOffset DataExpiracaoToken { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
