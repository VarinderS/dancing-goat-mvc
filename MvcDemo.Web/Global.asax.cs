using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using Kentico.Web.Mvc;
using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web
{
    public class MvcDemoApplication : HttpApplication
    {
        public const string INDEX_NAME = "TestMvcDemo.Index";
        public const string SITE_NAME = "TestMvcDemo";


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Enable localization of ASP.NET MVC model validation
            LocalizationConfig.RegisterLocalizationServices();

            // Configure the application to use the same smart search indexes as Kentico
            StorageMappingConfig.ConfigureStorageMapping();

            // Register dependency injection container
            RegisterAutofac();

            // Handle NotFound exceptions and display custom view instead of default IIS pages
            ControllerBuilder.Current.SetControllerFactory(
                new ControllerFactoryWrapper(
                    ControllerBuilder.Current.GetControllerFactory()
                )
            );
        }


        private void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Enable property injection in view pages
            builder.RegisterSource(new ViewRegistrationSource());

            // Register web abstraction classes
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register controllers
            builder.RegisterControllers(typeof(MvcDemoApplication).Assembly);

            // Register repositories
            builder.RegisterAssemblyTypes(typeof(MvcDemoApplication).Assembly).Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .WithParameter("siteName", SITE_NAME)
                .WithParameter((parameter, context) => parameter.Name == "cultureName", (parameter, context) => CultureInfo.CurrentUICulture.Name)
                .WithParameter((parameter, context) => parameter.Name == "latestVersionEnabled", (parameter, context) => IsPreviewEnabled())
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CachingRepositoryDecorator))
                .InstancePerRequest();

            // Register services
            builder.RegisterAssemblyTypes(typeof(MvcDemoApplication).Assembly).Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Service"))
                .WithParameter("siteName", SITE_NAME)
                .WithParameter("searchIndexName", INDEX_NAME)
                .WithParameter((parameter, context) => parameter.Name == "cultureName", (parameter, context) => CultureInfo.CurrentUICulture.Name)
                .InstancePerRequest();

            // Register providers of additional information about content items
            builder.RegisterType<ContentItemMetadataProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            // Register caching decorator for repositories
            builder.Register(context => new CachingRepositoryDecorator(SITE_NAME, GetCacheItemDuration(), context.Resolve<IContentItemMetadataProvider>(), IsCacheEnabled()))
                .InstancePerRequest();

            // Enable declaration of output cache dependencies in controllers
            builder.Register(context => new OutputCacheDependencies(SITE_NAME, context.Resolve<HttpResponseBase>(), context.Resolve<IContentItemMetadataProvider>(), IsCacheEnabled()))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Set the ASP.NET MVC dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }


        private static bool IsCacheEnabled()
        {
            return !IsPreviewEnabled();
        }


        private static bool IsPreviewEnabled()
        {
            return HttpContext.Current.Kentico().Preview().Enabled;
        }


        private static TimeSpan GetCacheItemDuration()
        {
            var value = ConfigurationManager.AppSettings["RepositoryCacheItemDuration"];
            var seconds = 0;

            if (Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out seconds) && seconds > 0)
            {
                return TimeSpan.FromSeconds(seconds);
            }

            return TimeSpan.Zero;
        }
    }
}