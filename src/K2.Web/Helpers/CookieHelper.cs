using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace K2.Web.Helpers
{
    /// <summary>
    /// Classe utilizada para extrair informações do cookie de autenticação
    /// </summary>
    public class CookieHelper
    {
        private readonly HttpContext _context;

        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// Obtém do ID do usuário
        /// </summary>
        public int ObterIdUsuario() => Convert.ToInt32(_context.User.Claims.First(x => x.Type == "IdUsuario").Value);

        /// <summary>
        /// Obtém o nome do usuário
        /// </summary>
        public string ObterNomeUsuario() => _context.User.Claims.First(x => x.Type == "Nome").Value;

        /// <summary>
        /// Obtém o primeiro nome do usuário
        /// </summary>
        /// <returns></returns>
        public string ObterPrimeiroNomeUsuario() => ObterNomeUsuario().Split(" ".ToCharArray())[0];

        /// <summary>
        /// Obtém o e-mail do usuário
        /// </summary>
        /// <returns></returns>
        public string ObterEmailUsuario() => _context.User.Claims.ElementAt(1).Value;

        /// <summary>
        /// Obtém o CPF do usuário
        /// </summary>
        public string ObterCpfUsuario() => _context.User.Claims.First(x => x.Type == "Cpf").Value;

        /// <summary>
        /// Obtém o RG do usuário
        /// </summary>
        public string ObterRgUsuario() => _context.User.Claims.First(x => x.Type == "Rg").Value;

        /// <summary>
        /// Obtém o celular do usuário
        /// </summary>
        public string ObterCelularUsuario() => _context.User.Claims.First(x => x.Type == "Celular").Value;


        /// <summary>
        /// Obtém o perfil associado ao usuário
        /// </summary>
        public string ObterPerfilUsuario()
        {
            return _context.User.Claims.First(x => x.Type == "Perfil").Value;
        }

        /// <summary>
        /// Obtém o nome do perfil associado ao usuário
        /// </summary>
        public string ObterNomePerfilUsuario()
        {
            switch (ObterPerfilUsuario())
            {
                case TipoPerfil.Administrador:
                case TipoPerfil.Motorista:
                    return ObterPerfilUsuario();
                case TipoPerfil.ProprietarioCarro:
                    return "Proprietário de carro";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Obtém o token JWT associado ao cookie
        /// </summary>
        public string ObterTokenJwt() => _context.User.Claims.First(x => x.Type == "jwtToken").Value;
    }
}
