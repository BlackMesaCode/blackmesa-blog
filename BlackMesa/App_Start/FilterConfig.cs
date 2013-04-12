using System.Web;
using System.Web.Mvc;
using BlackMesa.Utilities;

namespace BlackMesa
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckUnderConstruction());
            filters.Add(new CheckUnderMaintenance());
        }
    }
}