using CMS.Helpers;

namespace MvcDemo.Web.Services
{
    /// <summary>
    /// Localization Service.
    /// </summary>
    public class LocalizationService
    {
        /// <summary>
        /// Returns localization for given key.
        /// </summary>
        /// <param name="key">Resource string key.</param>
        /// <returns>Resource string for given key. Returns key if translation not found.</returns>
        public virtual string GetString(string key)
        {
            return ResHelper.GetString(key);
        }


        /// <summary>
        /// Replaces the format item in a specified resource string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="key">A key of resource string used as format string.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        /// <returns>A resource string in which the format items have been replaced by the string representation of the corresponding objects in arguments.</returns>
        public virtual string FormatString(string key, params object[] arguments)
        {
            var format = GetString(key);

            return string.Format(format, arguments);
        }


        /// <summary>
        /// Replaces "{$stringname$}" expressions in given text with localized strings using current culture.
        /// </summary>
        /// <param name="text">Text to be localized.</param>
        /// <returns>Text with localization macros replaced using respective resource strings.</returns>
        public virtual string LocalizeString(string text)
        {
            return ResHelper.LocalizeString(text);
        }
    }
}