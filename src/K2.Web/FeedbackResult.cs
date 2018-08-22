using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace K2.Web
{
    public class FeedbackResult : IActionResult
    {
        private readonly Feedback _feedback;
        private readonly TipoFeedbackResponse _tipoResponse;

        public FeedbackResult(Feedback feedback, TipoFeedbackResponse tipoResponse)
        {
            _feedback = feedback;
            _tipoResponse = tipoResponse;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (_tipoResponse == TipoFeedbackResponse.Html)
            {
                var viewResult = new ViewResult();
                viewResult.ViewName = "Feedback";
                viewResult.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);
                viewResult.ViewData.Model = _feedback;

                await viewResult.ExecuteResultAsync(context);
            }
            else
            {
                var jsonResult = new JsonResult(_feedback);

                await jsonResult.ExecuteResultAsync(context);
            }
        }
    }

    public enum TipoFeedbackResponse
    {
        Json,
        Html
    }
}
