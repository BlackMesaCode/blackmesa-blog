using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }
        public List<BlackMesa.Website.Main.Models.Learning.Folder> Path { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }

    }
}