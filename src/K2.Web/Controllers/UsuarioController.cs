using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        public UsuarioController(CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [Route("login")]
        public IActionResult Login()
        {
            return User.Identity.IsAuthenticated
                ? (ActionResult)RedirectToAction("Index", "Viagem")
                : View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de efetuar login.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> Login(string email, string senha, bool permanecerLogado)
        {
            var parametros = new Parameter[]
            {
                new Parameter{ Name = "email", Value = email, Type = ParameterType.QueryString },
                new Parameter{ Name = "senha", Value = senha, Type = ParameterType.QueryString }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/autenticar", Method.POST, parametros, false);

            var saida = AutenticarSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível efetuar o login.", saida.Mensagens));

            // Cria o cookie de autenticação

            var claims = new List<Claim>(saida.ObterClaims());
            claims.Add(new Claim("jwtToken", saida.ObterToken()));

            var userIdentity = new ClaimsIdentity(
                new GenericIdentity(saida.ObterNomeUsuario()),
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                null,
                null);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = permanecerLogado,
                ExpiresUtc = saida.Retorno.DataExpiracaoToken
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Usuário autenticado com sucesso."));
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new EmptyResult();
        }

        [HttpGet]
        [Route("alterar-senha")]
        public IActionResult AlterarSenha()
        {
            return PartialView();
        }

        [HttpPost]
        [Route("alterar-senha")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de alterar a senha de acesso.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarSenha(string senhaAtual, string senhaNova, string confirmacaoSenhaNova, bool enviarEmailSenhaNova)
        {
            var entrada = new AlterarSenhaUsuarioEntrada
            {
                SenhaAtual           = senhaAtual,
                SenhaNova            = senhaNova,
                ConfirmacaoSenhaNova = confirmacaoSenhaNova,
                EnviarEmailSenhaNova = enviarEmailSenhaNova
            };

            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/alterar-senha", Method.PUT, new[] { new Parameter { Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" } });

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar sua senha de acesso.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("redefinir-senha/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de redefinir a senha de acesso.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> RedefinirSenha(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/redefinir-senha/" + id, Method.PUT);

            var saida = Saida.Obter(apiResponse.Content);

            var senhaTemporaria = string.Empty;

            if (saida.Sucesso)
            {
                dynamic retorno = saida.Retorno;

                senhaTemporaria = retorno.senhaTemporaria;
            }

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar sua senha de acesso.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), !string.IsNullOrEmpty(senhaTemporaria)
                    ? new[] { $"A senha temporária redefinida é <b>{senhaTemporaria}</b>." }
                    : null, tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("redefinir-senha/{email}")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de enviar uma nova senha de acesso por e-mail.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> RedefinirSenha(string email)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/redefinir-senha/" + email, Method.PUT, usarToken: false);

            var saida = Saida.Obter(apiResponse.Content);

            var senhaTemporaria = string.Empty;

            if (saida.Sucesso)
            {
                dynamic retorno = saida.Retorno;

                senhaTemporaria = retorno.senhaTemporaria;
            }

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível enviar uma nova senha de acesso.", saida.Mensagens))
                : !string.IsNullOrEmpty(senhaTemporaria)
                    ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível enviar uma nova senha de acesso.", saida.Mensagens))
                    : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First()));
        }

        [HttpGet]
        [Route("alterar-meus-dados")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de obter seus dados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarMeusDados()
        {
            if (_cookieHelper.ObterPerfilUsuario() == TipoPerfil.Motorista)
            {
                var apiResponse = await _restSharpHelper.ChamarApi("usuarios/obter-por-id-usuario/" + _cookieHelper.ObterIdUsuario(), Method.GET);

                var saida = MotoristaSaida.Obter(apiResponse.Content);

                if (!saida.Sucesso)
                    return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível obter suas informações.", saida.Mensagens));

                ViewData["Cnh"] = saida.Retorno.Cnh;
            }

            return PartialView("AlterarMeusDados");
        }

        [HttpPost]
        [Route("alterar-meus-dados")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de alterar seus dados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarMeusDados(AlterarMeusDadosEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Os dados não foram preenchidos.", new[] { "Verifique se todas as informações foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/alterar-meus-dados", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida.Sucesso)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar os seus dados.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Seus dados foram alterados com sucesso.", new[] { "Você será redirecionado para a tela de login. Por favor faça login novamente." }, tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaLogin));
        }
    }
}