using BlackMesa.Blog.Models;
using PagedList;

namespace BlackMesa.Blog.ViewModels
{
    public class EntryIndexViewModel
    {
        public string OrderBy { get; set; }
        public int? Page { get; set; }
        public string SelectedTags { get; set; }
        public IPagedList<Entry> Entries { get; set; }
    }
}