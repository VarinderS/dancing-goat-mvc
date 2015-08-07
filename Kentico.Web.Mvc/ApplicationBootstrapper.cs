using System.Web;

using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(Kentico.Web.Mvc.ApplicationBootstrapper), "Run")]

namespace Kentico.Web.Mvc
{
    /// <summary>
    /// Initializes Kentico integration with ASP.NET MVC. This class is for internal use only.
    /// </summary>
    public static class ApplicationBootstrapper
    {
        /// <summary>
        /// Runs the bootstrapper process.
        /// </summary>
        public static void Run()
        {
            DynamicModuleUtility.RegisterModule(typeof(ApplicationHttpModule));
        }
    }
}
