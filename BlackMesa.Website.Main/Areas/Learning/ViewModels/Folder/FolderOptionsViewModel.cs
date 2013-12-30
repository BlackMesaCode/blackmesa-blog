using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class FolderOptionsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Path { get; set; }

    }
}