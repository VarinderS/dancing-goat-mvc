using CMS.Base;
using CMS.IO;

namespace MvcDemo.Web.App_Start
{
    public class SmartSearchMappingConfig
    {
        /// <summary>
        /// Configures the MVC application to use the same smart search indexes as the CMS application.
        /// </summary>
        public static void ConfigureSmartSearchIndexesMapping()
        {
            var provider = new StorageProvider
            {
                // Re-map the smart search indexes location to the CMS application
                CustomRootPath = Path.Combine(SystemContext.WebApplicationPhysicalPath, @"..\..\CMSSolution\CMS\")
            };

            StorageHelper.MapStoragePath("~/App_Data/CMSModules/SmartSearch", provider);
        }
    }
}