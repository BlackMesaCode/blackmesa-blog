using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class DeleteSelectionViewModel
    {
        public string Id { get; set; }
        public List<Models.Learning.Folder> SelectedFolders { get; set; }
        public List<Models.Learning.Card> SelectedCards { get; set; }
        public int AffectedFolders { get; set; }
        public int AffectedCards { get; set; }
    }
}