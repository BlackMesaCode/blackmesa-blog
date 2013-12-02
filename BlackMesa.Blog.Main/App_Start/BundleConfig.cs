using System.Web.Optimization;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace BlackMesa.Blog.Main.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            var cssTransformer = new CssTransformer();
            var jsTransformer = new JsTransformer();
            var cssMinifier = new CssMinify();
            var jsMinifier = new JsMinify();
            var nullOrderer = new NullOrderer();

            // Script Bundles

//            const string jQueryCdnPath = "http://code.jquery.com/jquery-1.9.1.min.js";

            bundles.Add(new ScriptBundle("~/bundles/global")
                .Include("~/Content/scripts/jquery-{version}.js", "~/Content/scripts/jquery-ui-1.10.2.custom.js", "~/Content/bootstrap/js/bootstrap.js", "~/Content/scripts/shCore.js", "~/Content/scripts/shAutoloader.js", "~/Content/scripts/shLegacy.js", "~/Content/scripts/jquery.taghandler.js", "~/Content/scripts/global.js", "~/Content/scripts/jquery.unobtrusive*", "~/Content/scripts/jquery.validate*", "~/Content/scripts/google-analytics.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin")
                .Include("~/Content/bootstrap-datepicker/js/bootstrap-datepicker.js", "~/Content/scripts/jquery-autogrow-textarea.js", "~/Content/scripts/ace/ace.js", "~/Content/scripts/ace/theme-clouds.js", "~/Content/scripts/ace/mode-html.js"));
            
 
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Content/scripts/modernizr-*"));


            // Style Bundles

            var mainBundle = new Bundle("~/Content/global")
                .Include("~/Content/font-awesome/css/font-awesome.min.css")
                .Include("~/Content/bootstrap/css/bootstrap.min.css")
                .Include("~/Content/custom/less/custom.less")
                .Include("~/Content/custom/less/jquery-ui-1.10.2.custom.less")
                .Include("~/Content/custom/less/shCoreDefault.less")
                .Include("~/Content/custom/less/shThemeDefault.less")
                .Include("~/Content/custom/less/jquery.taghandler.less");
                
            mainBundle.Transforms.Add(cssTransformer);
            mainBundle.Transforms.Add(cssMinifier);
            mainBundle.Orderer = nullOrderer;
            bundles.Add(mainBundle);

            var adminBundle = new Bundle("~/Content/admin")
                .Include("~/Content/bootstrap-datepicker/css/datepicker.css");
                //.Include("~/Content/styles/bootstrap-tagmanager.less");
            adminBundle.Transforms.Add(cssTransformer);
            mainBundle.Transforms.Add(cssMinifier);
            adminBundle.Orderer = nullOrderer;
            bundles.Add(adminBundle);


            //BundleTable.EnableOptimizations = true;  // executing this line will force bundling and minification by overwriting whatever stands in web.config
//            #if DEBUG
//                BundleTable.EnableOptimizations = false;
//            #endif

        }
    }
}