using System.Collections.Generic;
using System.Web.Mvc;

namespace BlackMesa.Blog.ViewModels
{
    public class EntryArchiveViewModel
    {
        public IEnumerable<SelectListItem> AvailableMonths { get; set; }
        public int SelectedMonth { get; set; }
        public IEnumerable<SelectListItem> AvailableYears { get; set; }
        public int SelectedYear { get; set; }
        public Dictionary<string, int> Tags { get; set; }
    }
}