using System.Web.Mvc;
using BlackMesa.Blog.Utilities;

namespace BlackMesa.Blog.App_Start
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