using System.Web.Mvc;

using MvcDemo.Web.Services;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Represents a view with localization service available.
    /// </summary>
    public abstract class CustomViewPage : WebViewPage
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