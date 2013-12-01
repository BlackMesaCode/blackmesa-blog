using System.Web.Mvc;
using Blog.Main.Utilities;

namespace Blog.Main.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckForMaintenance());
        }
    }
}