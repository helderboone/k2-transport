using K2.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace K2.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {

        }

        [AllowAnonymous]
        [Route("hello")]
        [HttpGet]
        public IActionResult Hello()
        {
            return NoContent();
        }
    }
}
