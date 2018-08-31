using Microsoft.AspNetCore.Mvc;
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
            var jsonResult = new JsonResult(_feedback);

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}
