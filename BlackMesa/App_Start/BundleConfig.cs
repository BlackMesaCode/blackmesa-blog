using System.Web.Optimization;
using BundleTransformer.Core.Minifiers;
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

//            const string jQueryCdnPath = "http://code.jquery.com/jquery-1.9.1.min.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.unobtrusive*","~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js", "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/texteditor")
                .Include("~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/magicsuggest")
                .Include("~/Scripts/magicsuggest-1.2.7.js"));
            
       
            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            // Style Bundles

            var mainBundle = new Bundle("~/Content/main")
                .Include("~/Content/less/bootstrap.less");
            mainBundle.Transforms.Add(cssTransformer);
            mainBundle.Transforms.Add(new CssMinify());
            mainBundle.Orderer = nullOrderer;
            bundles.Add(mainBundle);

            var adminBundle = new Bundle("~/Content/admin")
                .Include("~/Content/less/bootstrap-datetimepicker.less")
                .Include("~/Content/less/bootstrap-tagmanager.less");
            mainBundle.Transforms.Add(cssTransformer);
            mainBundle.Transforms.Add(new CssMinify());
            mainBundle.Orderer = nullOrderer;
            bundles.Add(adminBundle);


//            BundleTable.EnableOptimizations = true;  // executing this line will force bundling and minification by overwriting whatever stands in web.config
//            #if DEBUG
//                BundleTable.EnableOptimizations = false;
//            #endif

        }
    }
}