using System.Web.Mvc;

using MvcDemo.Web.Services;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Represents the services thar are needed in order to render a view.
    /// </summary>
    /// <typeparam name="T">The type of the view model.</typeparam>
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