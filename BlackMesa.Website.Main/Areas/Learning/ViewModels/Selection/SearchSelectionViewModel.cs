using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SearchSelectionViewModel
    {
        public string FolderId { get; set; }
        public string SearchText { get; set; }
        public bool SearchFrontSide { get; set; }
        public bool SearchBackSide { get; set; }
    }
}