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

            routes.MapRoute(
                name: "Entry",
                url: "entry/{id}/{title}",
                defaults: new { controller = "Entry", action = "Details", title = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Entry", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}