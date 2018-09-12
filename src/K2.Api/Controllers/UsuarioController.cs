using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class UsuarioController : BaseController
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
        public async Task<Models.Saida> Autenticar(
            string email,
            string senha,
            [FromServices] JwtTokenConfig tokenConfig /*FromServices: resolvidos via mecanismo de injeção de dependências do ASP.NET Core*/)
        {
            var entrada = new AutenticarUsuarioEntrada(email, senha);

            var saida = await _usuarioServico.Autenticar(entrada);

            if (!saida.Sucesso)
                return new Models.Saida(saida.Sucesso, saida.Mensagens, saida.Retorno);

            var usuario = (UsuarioSaida)saida.Retorno;

            var dataCriacaoToken = DateTime.Now;
            var dataExpiracaoToken = dataCriacaoToken + TimeSpan.FromDays(tokenConfig.ExpiracaoEmDias);

            return CriarResponseTokenJwt(usuario, dataCriacaoToken, dataExpiracaoToken, tokenConfig);
        }

        /// <summary>
        /// Realiza a alteração da senha de acesso do usuário
        /// </summary>
        [Authorize]
        [HttpPut]
        [Route("v1/usuarios/alterar-senha")]
        public async Task<Models.Saida> AlteraSenha([FromBody] Models.AlterarSenhaUsuarioEntrada model)
        {
            var emailUsuario = base.ObterEmailUsuarioAutenticado();

            var entrada = new AlterarSenhaUsuarioEntrada(emailUsuario, model.SenhaAtual, model.SenhaNova, model.ConfirmacaoSenhaNova, model.EnviarEmailSenhaNova);

            var saida = await _usuarioServico.AlterarSenha(entrada);

            return new Models.Saida(saida.Sucesso, saida.Mensagens, saida.Retorno);
        }

        private Models.Saida CriarResponseTokenJwt(UsuarioSaida usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, JwtTokenConfig tokenConfig)
        {
            var identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Email),
                    // Geração de claims. No contexto desse sistema, claims não precisaram ser criadas.
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                        new Claim("IdUsuario", usuario.Id.ToString()),
                        new Claim("Nome", usuario.Nome),
                        new Claim("Cpf", usuario.Cpf),
                        new Claim("Rg", usuario.Rg),
                        new Claim("Perfil", usuario.Perfil)
                    }
                );

            var jwtHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer             = tokenConfig.Issuer,
                Audience           = tokenConfig.Audience,
                SigningCredentials = tokenConfig.SigningCredentials,
                Subject            = identity,
                NotBefore          = dataCriacaoToken,
                Expires            = dataExpiracaoToken
            });

            // Cria o token JWT em formato de string
            var jwtToken = jwtHandler.WriteToken(securityToken);

            return new Models.AutenticacaoSaida(true, new[] { UsuarioResource.Usuario_Autenticado_Com_Sucesso }, new Models.Retorno(dataCriacaoToken, dataExpiracaoToken, jwtToken));
        }
    }
}