using System.Collections.Generic;
using System.Web.Mvc;

namespace BlackMesa.Website.Main.ViewModels
{
    public class EntryArchiveViewModel
    {
        public IEnumerable<SelectListItem> AvailableMonths { get; set; }
        public IEnumerable<SelectListItem> AvailableYears { get; set; }
    }
}