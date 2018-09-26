using K2.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly CookieHelper _cookieHelper;

        private readonly RestSharpHelper _restSharpHelper;

        public BaseController(CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
        {
            _cookieHelper = cookieHelper;
            _restSharpHelper = restSharpHelper;
        }

        public async Task<IRestResponse> ChamarApi(string rota, Method metodo, ICollection<Parameter> parametros = null, bool usarToken = true)
        {
            return await _restSharpHelper.ChamarApi(rota, metodo, parametros, usarToken);
        }
    }
}
