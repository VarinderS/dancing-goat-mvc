using System;
using System.Web.Mvc;

using CMS.Helpers;

namespace MvcDemo.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates an absolute URL of the given attachment.
        /// </summary>
        /// <param name="urlHelper">The UrlHelper instance that this method extends</param>
        /// <param name="attachmentGuid">The attachment unique identifier</param>
        /// <param name="fileName">Name of the file</param>
        public static string Attachment(this UrlHelper urlHelper, Guid attachmentGuid, string fileName)
        {
            fileName = fileName ?? string.Empty;

            var url = CMS.DocumentEngine.AttachmentInfoProvider.GetAttachmentUrl(attachmentGuid, fileName);
            return URLHelper.GetAbsoluteUrl(url, "127.0.0.77/CMS");
        }
    }
}