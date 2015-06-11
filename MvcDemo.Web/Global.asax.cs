using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using CMS.DataEngine;

using Autofac;
using Autofac.Integration.Mvc;
using MvcDemo.Web.Infrastructure;
using MvcDemo.Web.Repositories;
using MvcDemo.Web.Services;

using LocalizationService = MvcDemo.Web.Services.LocalizationService;
using MvcDemo.Web.Repositories.Implementation;

namespace MvcDemo.Web
{
    public class MvcApplication : HttpApplication
    {
        public const string INDEX_NAME = "TestMvcDemo.Index";
        public const string SITE_NAME = "TestMvcDemo";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Enable localization for the data annotation attributes
            LocalizationConfig.RegisterLocalizationServices();

            // Configure the MVC application to use the same smart search indexes as the CMS application
            StorageMappingConfig.ConfigureStorageMapping();

            // Register dependency injection container
            RegisterAutofac();

            // Handle NotFound exceptions and display custom view instead of default IIS pages.
            ControllerBuilder.Current.SetControllerFactory(
                new ControllerFactoryWrapper(
                    ControllerBuilder.Current.GetControllerFactory()
                )
            );
        }


        protected void Application_BeginRequest()
        {
            CMSApplication.Init();
        }


        private void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Register Property Injection for View Pages
            builder.RegisterSource(new ViewRegistrationSource());

            // Register MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register repositories
            builder.Register(context => new KenticoArticleRepository(SITE_NAME, CultureInfo.CurrentUICulture.Name)).As<ArticleRepository>().InstancePerRequest();
            builder.Register(context => new KenticoFormItemRepository()).As<FormItemRepository>().InstancePerRequest();
            builder.Register(context => new KenticoCafeRepository(SITE_NAME, CultureInfo.CurrentUICulture.Name)).As<CafeRepository>().InstancePerRequest();
            builder.Register(context => new KenticoAboutUsRepository(SITE_NAME, CultureInfo.CurrentUICulture.Name)).As<AboutUsRepository>().InstancePerRequest();
            builder.Register(context => new KenticoContactRepository(SITE_NAME, CultureInfo.CurrentUICulture.Name)).As<ContactRepository>().InstancePerRequest();
            builder.Register(context => new KenticoSocialLinkRepository(SITE_NAME, CultureInfo.CurrentUICulture.Name)).As<SocialLinkRepository>().InstancePerRequest();
            builder.Register(context => new KenticoCountryRepository()).As<CountryRepository>().InstancePerRequest();


            // Register services - the most general registration is first
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).Where(t => t.Name.EndsWith("Service"));
            builder.Register(context => new SearchService(INDEX_NAME, CultureInfo.CurrentUICulture.Name, SITE_NAME)).InstancePerRequest();
                                            
            // Register property injection for LocalizationService
            builder.RegisterType<LocalizationService>().PropertiesAutowired();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
