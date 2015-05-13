using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using CMS.DataEngine;

using MvcDemo.Web.App_Start;
using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Enable localization for the data annotation attributes
            LocalizationConfig.ConfigureLocalization();

            // Setup a fall-back controller for 404
            ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));

            // Configure the MVC application to use the same smart search indexes as the CMS application
            SmartSearchMappingConfig.ConfigureSmartSearchIndexesMapping();
        }


        protected void Application_BeginRequest()
        {
            CMSApplication.Init();
        }
    }
}
