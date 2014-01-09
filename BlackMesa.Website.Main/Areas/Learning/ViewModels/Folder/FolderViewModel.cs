using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class FolderViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsRootFolder { get; set; }
        public int Level { get; set; }
        public bool HasAnySelection { get; set; }
        public string ParentFolderId { get; set; }
        public List<BlackMesa.Website.Main.Models.Learning.Folder> Path { get; set; }
        public IEnumerable<BlackMesa.Website.Main.Models.Learning.Folder> SubFolders { get; set; }
        public IEnumerable<BlackMesa.Website.Main.Models.Learning.Card> Cards { get; set; }
    }
}