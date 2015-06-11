using CMS.Base;
using CMS.IO;

namespace MvcDemo.Web
{
    /// <summary>
    /// Enables working with files that are not a part of this application.
    /// </summary>
    public class StorageMappingConfig
    {
        /// <summary>
        /// Enables working with files from Kentico web application.
        /// </summary>
        public static void ConfigureStorageMapping()
        {
            var kenticoRootPath = Path.Combine(SystemContext.WebApplicationPhysicalPath, @"..\..\CMSSolution\CMS\");

            ConfigureSmartSearchStorageMapping(kenticoRootPath);
        }


        /// <summary>
        /// Enables working with smart search index from Kentico web application.
        /// </summary>
        /// <param name="kenticoRootPath">The absolute path to the folder with Kentico web application.</param>
        private static void ConfigureSmartSearchStorageMapping(string kenticoRootPath)
        {
            var storageProvider = new StorageProvider
            {
                CustomRootPath = kenticoRootPath
            };
            StorageHelper.MapStoragePath("~/App_Data/CMSModules/SmartSearch", storageProvider);
        }
    }
}