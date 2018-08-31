using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace K2.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;

        protected readonly ILogger _logger;

        protected readonly RestClient _restClient;

        public BaseController(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;

            _logger = logger;

            _restClient = new RestClient(configuration["UrlApi"]);
        }
    }
}
