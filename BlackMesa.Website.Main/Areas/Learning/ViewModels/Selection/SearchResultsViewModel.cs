using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SearchResultsViewModel
    {
        public string Id { get; set; }
        public List<SearchResultViewModel> SearchResults { get; set; }
    }
}