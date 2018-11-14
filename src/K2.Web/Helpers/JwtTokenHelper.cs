using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace K2.Web.Helpers
{
    public static class JwtTokenHelper
    {
        /// <summary>
        /// Extrai as claims de um token JWT
        /// </summary>
        public static IEnumerable<Claim> ExtrairClaims(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new List<Claim>();

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(jwtToken))
                return new List<Claim>();

            var jwtSecurityToken = jwtHandler.ReadJwtToken(jwtToken);

            return jwtSecurityToken?.Claims;
        }
    }
}
