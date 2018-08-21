using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace K2.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;

        protected readonly RestClient _restClient;

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;

            _restClient = new RestClient(configuration["UrlApi"]);
        }
    }
}
