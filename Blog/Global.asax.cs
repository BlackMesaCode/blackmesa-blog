using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BlackMesa.App_Start;

namespace BlackMesa
{
    public static class Global
    {
        public static readonly string CultureConstraints = @"\w{2,3}(-\w{4})?(-\w{2,3})?";
        public static readonly string LanguageConstraints = @"\w{2}";
        public static readonly object IdConstraints = @"\d+";
        public static readonly List<string> AllowedCultures = new List<string> { "en-US", "de-DE", "de-CH", "de-AT", "de-LI", "de-LU" };
        public static readonly List<string> AllowedLanguages = new List<string> { "en", "de" };
        public static readonly TimeZoneInfo ApplicationTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    }

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            EntityFrameworkProfilerBootstrapper.PreStart();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }


    }
}