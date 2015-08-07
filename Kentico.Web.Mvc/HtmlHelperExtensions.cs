using System;
using System.Web.Mvc;

namespace Kentico.Web.Mvc
{
    /// <summary>
    /// Provides methods to structure Kentico content.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        private static ExtensionPoint<HtmlHelper> mExtensionPoint;
        private static readonly object mLock = new Object();


        /// <summary>
        /// Returns an object that provides methods to structure Kentico content.
        /// </summary>
        /// <param name="target">The instance of the <see cref="System.Web.Mvc.HtmlHelper"/> class.</param>
        /// <returns>The object that provides methods to structure Kentico content.</returns>
        public static ExtensionPoint<HtmlHelper> Kentico(this HtmlHelper target)
        {
            lock (mLock)
            {
                if (mExtensionPoint == null || mExtensionPoint.Target != target)
                {
                    mExtensionPoint = new ExtensionPoint<HtmlHelper>(target);
                }

                return mExtensionPoint;
            }
        }
    }
}
