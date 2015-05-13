using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using MvcDemo.Web.Controllers;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Custom controller factory which handles 404 redirection to the custom controller.
    /// </summary>
    public class ControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
            catch (HttpException exception)
            {
                // Use ErrorController for URLs which doesn't match any corresponding controller
                if (exception.GetHttpCode() == 404)
                {
                    return new ErrorController();
                }

                throw;
            }
        }
    }
}