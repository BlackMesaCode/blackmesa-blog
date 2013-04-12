using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackMesa.Utilities
{
    public sealed class CheckUnderMaintenance : ActionFilterAttribute
    {

        private static List<string> _allowedIps = new List<string>
                                                      {
                                                          "134.3.100.237"
                                                      };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ConfigurationManager.AppSettings["UnderMaintenance"] == "true" && !filterContext.RouteData.Values["controller"].Equals("UnderMaintenance"))
            {
                if (!filterContext.HttpContext.Request.IsLocal && !_allowedIps.Contains(filterContext.HttpContext.Request.UserHostAddress))
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new { controller = "UnderMaintenance", action = "Index" });
                }
            }

            base.OnActionExecuting(filterContext);
        }


    }
}