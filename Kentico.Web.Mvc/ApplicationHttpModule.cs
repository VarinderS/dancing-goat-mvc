﻿using System;
using System.Web;
using System.Web.Helpers;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.DocumentEngine;

namespace Kentico.Web.Mvc
{
    /// <summary>
    /// Provides Kentico integration with ASP.NET MVC.
    /// </summary>
    internal sealed class ApplicationHttpModule : IHttpModule
    {
        /// <summary>
        /// Releases the resources used by a module.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose
        }


        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="application">An object that provides access to the methods, properties, and events common to all application objects within an ASP.NET application.</param>
        public void Init(HttpApplication application)
        {
            application.BeginRequest += HandleBeginRequest;
        }


        private void HandleBeginRequest(object sender, EventArgs e)
        {
            CMSApplication.Init();

            var application = (HttpApplication)sender;
            var context = application.Context;
            var relativeFilePath = context.Request.AppRelativeCurrentExecutionFilePath.TrimStart('~');

            // Check whether the current request URL contains information about a preview mode
            if (VirtualContext.HandleVirtualContext(ref relativeFilePath))
            {
                // Validate integrity of preview information (including the unique identifier generated by Kentico) in the current request URL
                if (!VirtualContext.ValidatePreviewHash(relativeFilePath) || !ValidatePreviewGuid(VirtualContext.GetItem(VirtualContext.PARAM_WF_GUID)))
                {
                    VirtualContext.Reset();
                    throw new HttpException(404, "The preview link is not valid.");
                }

                // Disable same origin policy for a preview mode as Kentico displays preview in a frame
                AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

                // Do not cache response in a preview mode, it has to contain current data
                context.Response.Cache.SetNoServerCaching();
                context.Response.Cache.SetNoStore();

                // Remove preview mode information from the request URL
                context.RewritePath("~" + relativeFilePath, context.Request.PathInfo, context.Request.Url.Query.TrimStart('?'));
            }
            else
            {
                // Add validation callback for the output cache item to ignore it in a preview mode
                context.Response.Cache.AddValidationCallback(ValidateCacheItem, null);
            }

            var previewFeature = new PreviewFeature();
            context.Kentico().SetFeature<IPreviewFeature>(previewFeature);
        }


        private static bool ValidatePreviewGuid(object value)
        {
            var identifier = ValidationHelper.GetGuid(value, Guid.Empty);
            var query = new DocumentQuery().WhereEquals("DocumentWorkflowCycleGUID", identifier);

            return query.HasResults();
        }


        private static void ValidateCacheItem(HttpContext context, object data, ref HttpValidationStatus status)
        {
            if (context.Kentico().Preview().Enabled)
            {
                status = HttpValidationStatus.IgnoreThisRequest;
            }
        }
    }
}
