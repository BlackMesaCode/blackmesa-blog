using System.Web.Optimization;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace BlackMesa
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            var cssTransformer = new CssTransformer();
            var jsTransformer = new JsTransformer();
            var nullOrderer = new NullOrderer();

            // Script Bundles

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));




            // Style Bundles

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            var lessBundle = new Bundle("~/Content/css").Include("~/Content/less/bootstrap/bootstrap.less", "~/Content/less/blackfor.less");
            lessBundle.Transforms.Add(cssTransformer);
            lessBundle.Transforms.Add(new CssMinify());
            lessBundle.Orderer = nullOrderer;
            bundles.Add(lessBundle);

//            BundleTable.EnableOptimizations = true;  // executing this line will force bundling and minification by overwriting whatever stands in web.config
//            #if DEBUG
//                BundleTable.EnableOptimizations = false;
//            #endif

        }
    }
}