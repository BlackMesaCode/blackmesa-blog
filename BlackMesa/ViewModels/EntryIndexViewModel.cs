using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Models;

namespace BlackMesa.ViewModels
{
    public class EntryIndexViewModel
    {
        public string OrderBy { get; set; }
        public string SelectedTags { get; set; }
        public ICollection<Entry> Entries { get; set; }
    }
}