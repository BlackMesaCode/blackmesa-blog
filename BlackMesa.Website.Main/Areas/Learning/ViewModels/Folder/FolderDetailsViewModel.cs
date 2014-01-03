using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class FolderDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public int Level { get; set; }
        public bool HasAnySelection { get; set; }
        public bool HasAnyFolderSelection { get; set; }
        public Dictionary<string, string> Path { get; set; }
        public List<BlackMesa.Website.Main.Models.Learning.Folder> SubFolders { get; set; }
        public IEnumerable<BlackMesa.Website.Main.Models.Learning.IndexCard> IndexCards { get; set; }
    }
}