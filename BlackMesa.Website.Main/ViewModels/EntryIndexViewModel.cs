﻿using BlackMesa.Blog.Model;
using PagedList;

namespace BlackMesa.Website.Main.ViewModels
{
    public class EntryIndexViewModel
    {
        public string OrderBy { get; set; }
        public string SelectedTag { get; set; }
        public int? SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }
        public int EntriesFound { get; set; }
        public IPagedList<Entry> Entries { get; set; }
    }
}