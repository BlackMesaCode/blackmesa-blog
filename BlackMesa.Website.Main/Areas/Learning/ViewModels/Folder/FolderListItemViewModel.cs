using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class FolderListItemViewModel
    {
        public string Id { get; set; }

        public string ParentFolderId { get; set; }
        
        public string Name { get; set; }

        public int Level { get; set; }

        public bool IsSelected { get; set; }

        public int NumberOfLearningUnitsInSameFolder { get; set; }

        public int NumberOfLearningUnitsIncludingAllSubfolders { get; set; }

        public List<FolderListItemViewModel> SubFolders { get; set; }

    }
}