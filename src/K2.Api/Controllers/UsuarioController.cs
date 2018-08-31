using K2.Dominio.Comandos.Entrada.Usuario;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        /// <summary>
        /// Realiza a autenticação do usuário, a partir do e-mail e senha informados.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("v1/usuarios/autenticar")]
        public async Task<ISaida> Autenticar(
            string email,
            string senha,
            [FromServices] JwtTokenConfig tokenConfig /*FromServices: resolvidos via mecanismo de injeção de dependências do ASP.NET Core*/)
        {
            var autenticarComando = new AutenticarUsuarioEntrada(email, senha);

            var comandoSaida = await _usuarioServico.Autenticar(autenticarComando);

            if (!comandoSaida.Sucesso)
                return comandoSaida;

            var usuario = (UsuarioSaida)comandoSaida.Retorno;

            var dataCriacaoToken = DateTime.Now;
            var dataExpiracaoToken = dataCriacaoToken + TimeSpan.FromDays(tokenConfig.ExpiracaoEmDias);

            return CriarResponseTokenJwt(usuario, dataCriacaoToken, dataExpiracaoToken, tokenConfig);
        }

        private ISaida CriarResponseTokenJwt(UsuarioSaida usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, JwtTokenConfig tokenConfig)
        {
            var identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Nome),
                    // Geração de claims. No contexto desse sistema, claims não precisaram ser criadas.
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                        new Claim("IdUsuario", usuario.Id.ToString()),
                        new Claim("Nome", usuario.Nome),
                        new Claim("Cpf", usuario.Cpf),
                        new Claim("Rg", usuario.Rg)
                    }
                    // Adiciona os perfis de acesso do usuário
                    .Union(usuario.Perfis.Select(x => new Claim("Perfil", x)))
                );

            var jwtHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfig.Issuer,
                Audience = tokenConfig.Audience,
                SigningCredentials = tokenConfig.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacaoToken,
                Expires = dataExpiracaoToken
            });

            // Cria o token JWT em formato de string
            var jwtToken = jwtHandler.WriteToken(securityToken);

            return new Saida(true, new[] { UsuarioResource.Usuario_Autenticado_Com_Sucesso }, new
            {
                DataCriacaoToken = dataCriacaoToken,
                DataExpiracaoToken = dataExpiracaoToken,
                Token = jwtToken,
            });
        }
    }
}