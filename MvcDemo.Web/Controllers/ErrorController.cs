using System.Web.Mvc;

namespace MvcDemo.Web.Controllers
{
    /// <summary>
    /// Error controller is used as a fall-back controller when the request URL does not match any controller.
    /// </summary>
    public class ErrorController : BaseController
    {
        // GET: NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}