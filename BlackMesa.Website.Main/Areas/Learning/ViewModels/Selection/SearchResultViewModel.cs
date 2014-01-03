using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }
        public Dictionary<string, string> Path { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }

    }
}