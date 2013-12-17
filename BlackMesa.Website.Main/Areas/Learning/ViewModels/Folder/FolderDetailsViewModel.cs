using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class FolderDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<FolderListItemViewModel> SubFolders { get; set; }

        public IEnumerable<BlackMesa.Website.Main.Models.Learning.TextCard> TextCards { get; set; }
    }
}