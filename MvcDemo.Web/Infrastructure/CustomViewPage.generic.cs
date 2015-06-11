using System.Web.Mvc;

using MvcDemo.Web.Services;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Represents a view with localization service available.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CustomViewPage<T> : WebViewPage<T>
    {
        /// <summary>
        /// Gets or sets the localization service.
        /// </summary>
        public LocalizationService Resources
        {
            get;
            set;
        }
    }
}