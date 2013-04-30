using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Models;
using PagedList;

namespace BlackMesa.ViewModels
{
    public class EntryIndexViewModel
    {
        public string OrderBy { get; set; }
        public int? Page { get; set; }
        public string SelectedTags { get; set; }
        public IPagedList<Entry> Entries { get; set; }
    }
}