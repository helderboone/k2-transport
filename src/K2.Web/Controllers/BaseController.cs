using K2.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;

        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected readonly ILogger _logger;

        protected readonly RestClient _restClient;

        protected readonly CookieHelper _cookieHelper;

        public BaseController(IConfiguration configuration, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration       = configuration;
            _httpContextAccessor = httpContextAccessor;
            _cookieHelper        = new CookieHelper(httpContextAccessor.HttpContext);
            _logger              = logger;

            _restClient = new RestClient(configuration["UrlApi"]);
        }

        public async Task<IRestResponse> ChamarApi(string rota, Method metodo, ICollection<Parameter> parametros = null, bool usarToken = true)
        {
            try
            {
                var request = new RestRequest(rota, metodo);
                request.AddHeader("Content-Type", "application/json");

                if (usarToken)
                {
                    var tokenJwt = _cookieHelper.ObterTokenJwt();

                    if (!string.IsNullOrEmpty(tokenJwt))
                        request.AddHeader("Authorization", "Bearer " + tokenJwt);
                }

                if (parametros.Any())
                {
                    foreach (var parametro in parametros)
                        request.AddParameter(parametro);
                }

                var response = await _restClient.ExecuteTaskAsync(request);

                if (!response.IsSuccessful)
                    throw new Exception("Falha na comunicação com a API.", response.ErrorException);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha na comunicação com a API.", ex);
            }
        }
    }
}
