using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class FolderIndexViewModel
    {

        public int TotalLearningUnits { get; set; } 
        public List<FolderListItemViewModel> Folders { get; set; } 

    }
}