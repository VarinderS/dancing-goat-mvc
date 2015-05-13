using CMS.Helpers;

namespace MvcDemo.Web.Helpers
{
    /// <summary>
    /// Helper class providing localization.
    /// </summary>
    public static class Resource
    {
        /// <summary>
        /// Returns localization for given key.
        /// </summary>
        /// <param name="key">Resource string key.</param>
        /// <returns>Resource string for given key. Returns key if translation not found.</returns>
        public static string GetString(string key)
        {
            return ResHelper.GetString(key);
        }


        /// <summary>
        /// Replaces the format item in a specified resource string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="key">A key of resource string used as format string.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        /// <returns>A resource string in which the format items have been replaced by the string representation of the corresponding objects in arguments.</returns>
        public static string FormatString(string key, params object[] arguments)
        {
            var format = GetString(key);

            return string.Format(format, arguments);
        }
    }
}