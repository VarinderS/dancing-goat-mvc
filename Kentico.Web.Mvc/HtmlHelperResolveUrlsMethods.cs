using System.Text;
using System.Web.Mvc;
using CMS.Base;

namespace Kentico.Web.Mvc
{
    /// <summary>
    /// Provides methods to resolve urls which starts with '~' character.
    /// </summary>
    public static class HtmlHelperResolveUrlsMethods
    {
        /// <summary>
        /// Resolves all urls starting with '~' character to relative paths.
        /// </summary>
        /// <param name="instance">The object that provides methods to structure Kentico content.</param>
        /// <param name="html">Input html content.</param>
        /// <returns>Resolved urls to relative paths.</returns>
        public static MvcHtmlString ResolveUrls(this ExtensionPoint<HtmlHelper> instance, string html)
        {
            var urlHelper = new UrlHelper(instance.Target.ViewContext.RequestContext);
            var applicationPath = urlHelper.Content("~/").TrimEnd('/');

            var pathIndex = html.IndexOfCSafe("~/");
            if (pathIndex >= 1)
            {
                var sb = new StringBuilder((int)(html.Length * 1.1));

                var lastIndex = 0;
                while (pathIndex >= 1)
                {
                    if ((html[pathIndex - 1] == '(') || (html[pathIndex - 1] == '"') || (html[pathIndex - 1] == '\''))
                    {
                        // Add previous content
                        if (lastIndex < pathIndex)
                        {
                            sb.Append(html, lastIndex, pathIndex - lastIndex);
                        }

                        // Add application path and move to the next location
                        sb.Append(applicationPath);
                        lastIndex = pathIndex + 1;
                    }

                    pathIndex = html.IndexOfCSafe("~/", pathIndex + 2);
                }

                // Add the rest of the content
                if (lastIndex < html.Length)
                {
                    sb.Append(html, lastIndex, html.Length - lastIndex);
                }

                html = sb.ToString();
            }

            return new MvcHtmlString(html);
        }
    }
}
