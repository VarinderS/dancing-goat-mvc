using System.Web.Optimization;

namespace MvcDemo.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Enable CDN
            bundles.UseCdn = true;

            var jqueryCDN = "//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.3.min.js";
            var jquery = new ScriptBundle("~/bundles/jquery", jqueryCDN).Include(
                        "~/Scripts/jquery-{version}.js");
            
            jquery.CdnFallbackExpression = "window.jQuery";
            bundles.Add(jquery);

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js"));
        }
    }
}