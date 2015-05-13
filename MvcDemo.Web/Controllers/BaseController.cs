using System.Web.Mvc;
using System.Web.Routing;

namespace MvcDemo.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action</param>
        protected override void HandleUnknownAction(string actionName)
        {
            NotFoundResult();
        }


        /// <summary>
        /// Default 404 action.
        /// </summary>
        protected HttpNotFoundResult NotFoundResult()
        {
            var errorController = new ErrorController();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "NotFound");
            
            errorController.Execute(new RequestContext(HttpContext, errorRoute));

            return HttpNotFound();
        }
    }
}