using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace K2.Api.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Obtém do token JWT, o e-mail do usuário
        /// </summary>
        public string ObterEmailUsuarioAutenticado() => User.Identity.Name;

        /// <summary>
        /// Obtém do ID do usuário
        /// </summary>
        public int ObterIdUsuario() => Convert.ToInt32(User.Claims.First(x => x.Type == "IdUsuario").Value);

        /// <summary>
        /// Obtém o perfil associado ao usuário
        /// </summary>
        public string ObterPerfilUsuario() => User.Claims.First(x => x.Type == "Perfil").Value;
    }
}