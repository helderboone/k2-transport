using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace K2.Web
{
    public class FeedbackResult : IActionResult
    {
        private readonly Feedback _feedback;

        public FeedbackResult(Feedback feedback)
        {
            _feedback = feedback;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var jsonResult = new JsonResult(_feedback);

                await jsonResult.ExecuteResultAsync(context);
            }
            else
            {
                var viewResult = new ViewResult();
                viewResult.ViewName = "Feedback";
                viewResult.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);
                viewResult.ViewData.Model = _feedback;

                await viewResult.ExecuteResultAsync(context);
            }
        }
    }
}
