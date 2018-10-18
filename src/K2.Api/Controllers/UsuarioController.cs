using K2.Dominio;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Interfaces.Comandos;
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
        /// Realiza uma procura por usuarios a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("v1/usuarios/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarUsuarioEntrada entrada)
        {
            return await _usuarioServico.ProcurarUsuarios(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo usuario
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("v1/usuarios/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarUsuarioEntrada entrada)
        {
            return await _usuarioServico.CadastrarUsuario(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um usuario
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPut]
        [Route("v1/usuarios/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarUsuarioEntrada entrada)
        {
            return await _usuarioServico.AlterarUsuario(entrada);
        }

        /// <summary>
        /// Obtém um usuario a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("v1/usuarios/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _usuarioServico.ObterUsuarioPorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um usuario.
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpDelete]
        [Route("v1/usuarios/excluir/{id:int}")]
        public async Task<ISaida> ExcluirUsuario(int id)
        {
            return await _usuarioServico.ExcluirUsuario(id);
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
            var entrada = new AutenticarUsuarioEntrada(email, senha);

            var saida = await _usuarioServico.Autenticar(entrada);

            if (!saida.Sucesso)
                return new Saida(saida.Sucesso, saida.Mensagens, saida.Retorno);

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
        public async Task<ISaida> AlteraSenha([FromBody] AlterarSenhaUsuarioEntrada model)
        {
            var emailUsuario = base.ObterEmailUsuarioAutenticado();

            var entrada = new AlterarSenhaUsuarioEntrada(emailUsuario, model.SenhaAtual, model.SenhaNova, model.ConfirmacaoSenhaNova, model.EnviarEmailSenhaNova);

            return await _usuarioServico.AlterarSenha(entrada);
        }

        /// <summary>
        /// Redefine a senha de acesso do usuário para uma senha temporária
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPut]
        [Route("v1/usuarios/redefinir-senha/{id:int}")]
        public async Task<ISaida> RedefinirSenha(int id)
        {
            return await _usuarioServico.RedefinirSenha(id);
        }

        /// <summary>
        /// Redefine a senha de acesso do usuário para uma senha temporária
        /// </summary>
        [AllowAnonymous]
        [HttpPut]
        [Route("v1/usuarios/redefinir-senha/{email}")]
        public async Task<ISaida> RedefinirSenha(string email)
        {
            return await _usuarioServico.RedefinirSenha(email);
        }

        private ISaida CriarResponseTokenJwt(UsuarioSaida usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, JwtTokenConfig tokenConfig)
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

            return new Saida(
                true,
                new[] { UsuarioResource.Usuario_Autenticado_Com_Sucesso },
                new { DataCriacaoToken = dataCriacaoToken, DataExpiracaoToken = dataExpiracaoToken, Token = jwtToken });
        }
    }
}