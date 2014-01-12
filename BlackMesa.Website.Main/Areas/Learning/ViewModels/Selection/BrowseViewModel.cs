using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class BrowseViewModel
    {
        public string FolderId { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public int Position { get; set; }
        public int CardsCount { get; set; }
    }
}