using K2.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace K2.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly CookieHelper _cookieHelper;

        protected readonly RestSharpHelper _restSharpHelper;

        protected BaseController(CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
        {
            _cookieHelper = cookieHelper;
            _restSharpHelper = restSharpHelper;
        }
    }
}
