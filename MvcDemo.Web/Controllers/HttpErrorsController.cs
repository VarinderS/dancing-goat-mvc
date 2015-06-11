using System.Web.Mvc;

namespace MvcDemo.Web.Controllers
{
    public class HttpErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;

            return View();
        }
    }
}