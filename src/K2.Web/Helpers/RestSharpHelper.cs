using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Helpers
{
    public class RestSharpHelper
    {
        private readonly IConfiguration _configuration;
        private readonly CookieHelper _cookieHelper;
        private readonly RestClient _restClient;

        public RestSharpHelper(IConfiguration configuration, CookieHelper cookieHelper)
        {
            _configuration = configuration;
            _cookieHelper = cookieHelper;
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

                if (parametros != null && parametros.Any())
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
