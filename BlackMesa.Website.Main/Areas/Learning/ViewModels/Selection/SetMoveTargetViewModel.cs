using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SetMoveTargetViewModel
    {
        public string SourceFolderId { get; set; }
        public BlackMesa.Website.Main.Models.Learning.Folder SourceFolder { get; set; }
        public IEnumerable<BlackMesa.Website.Main.Models.Learning.Folder> Folders { get; set; }
    }
}