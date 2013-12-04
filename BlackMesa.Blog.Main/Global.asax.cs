using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BlackMesa.Blog.DataLayer;
using BlackMesa.Blog.Main.App_Start;
using BlackMesa.Blog.Main.Migrations;

namespace BlackMesa.Blog.Main
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
            // We dont need the initializer as we dont use AutomaticMigrations 
            // but instead manually add-migrations via the package manager console and also manually update-database
            //Database.SetInitializer(new CreateDatabaseIfNotExists<BlackMesaDb>());  

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // we can only call it here, if the database is already created
            // so if we create the database the first time, we have to comment out the line below
            AuthConfig.RegisterAuth();
        }


    }
}