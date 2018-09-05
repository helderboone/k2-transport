using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace K2.Web.Controllers
{
    public class ClienteController : BaseController
    {
        public ClienteController(IConfiguration configuration, ILogger<ClienteController> logger, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, httpContextAccessor)
        {

        }

        [Authorize(Policy = "Administrador")]
        [Route("clientes")]
        public IActionResult Index()
        {
            return View();
        }
    }
}