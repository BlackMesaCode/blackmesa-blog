using System.Web.Mvc;
using BlackMesa.Website.Main.Utilities;

namespace BlackMesa.Website.Main.App_Start
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