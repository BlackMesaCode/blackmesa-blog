using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackMesa.Utilities
{
    public sealed class CheckUnderConstruction : ActionFilterAttribute
    {

        private static List<string> _allowedIps = new List<string>
                                                      {
                                                          "134.3.100.237"
                                                      };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ConfigurationManager.AppSettings["UnderMaintenance"] == "true")
            {
                if (!filterContext.HttpContext.Request.IsLocal && !_allowedIps.Contains(filterContext.HttpContext.Request.UserHostAddress))
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new { controller = "UnderMaintenance", action = "Index" });
                }
            }
            else if (ConfigurationManager.AppSettings["UnderConstruction"] == "true" && !filterContext.RouteData.Values["controller"].Equals("UnderConstruction"))
            {
                if (!filterContext.HttpContext.Request.IsLocal && !_allowedIps.Contains(filterContext.HttpContext.Request.UserHostAddress))
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new { controller = "UnderConstruction", action = "Index" });
                }
            }

            base.OnActionExecuting(filterContext);
        }


    }
}