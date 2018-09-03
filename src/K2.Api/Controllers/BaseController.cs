using Microsoft.AspNetCore.Mvc;

namespace K2.Api.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Obtém do token JWT, o e-mail do usuário
        /// </summary>
        public string ObterEmailUsuarioAutenticado() => User.Identity.Name;
    }
}