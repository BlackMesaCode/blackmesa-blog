using BlackMesa.Blog.Models.Blog;
using PagedList;

namespace BlackMesa.Blog.ViewModels
{
    public class EntryIndexViewModel
    {
        public string OrderBy { get; set; }
        public string SelectedTag { get; set; }
        public int? SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }
        public string SearchText { get; set; }
        public int EntriesFound { get; set; }
        public IPagedList<Entry> Entries { get; set; }
    }
}