using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlackMesa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//            routes.MapRoute(
//                name: "Entry",
//                url: "{id}/{title}",
//                defaults: new { controller = "Entry", action = "Details", title = UrlParameter.Optional },
//                constraints: new { id = @"\d+" }
//            );
//
//            routes.MapRoute(
//                name: "Default",
//                url: "{controller}/{action}/{id}",
//                defaults: new { controller = "Entry", action = "Index", id = UrlParameter.Optional }
//            );


            // todo get accept-language from users browser and if i support this language, redirect to it, otherwise use default language

            routes.MapRoute(
                name: "Entry",
                url: "{language}/{id}/{title}",
                defaults: new { language = "de", controller = "Entry", action = "Details", title = UrlParameter.Optional },
                constraints: new { id = @"\d+", language = new FromValuesListConstraint("en", "de") }
            );

            routes.MapRoute(
                name: "Default",
                url: "{language}/{controller}/{action}/{id}",
                defaults: new { language = "de", controller = "Entry", action = "Index", id = UrlParameter.Optional },
                constraints: new { language = new FromValuesListConstraint("en", "de") }
            );

        }
    }


    public class FromValuesListConstraint : IRouteConstraint
    {
        public FromValuesListConstraint(params string[] values)
        {
            this._values = values;
        }

        private string[] _values;

        public bool Match(HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            // Get the value called "parameterName" from the 
            // RouteValueDictionary called "value"
            string value = values[parameterName].ToString();

            // Return true is the list of allowed values contains 
            // this value.
            return _values.Contains(value);
        }
    }
}