using System.Web.Mvc;
using BlackMesa.Blog.Main.Utilities;

namespace BlackMesa.Blog.Main.App_Start
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