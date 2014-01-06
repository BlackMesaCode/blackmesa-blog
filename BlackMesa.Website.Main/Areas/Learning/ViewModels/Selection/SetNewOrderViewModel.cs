using System.Collections.Generic;
using BlackMesa.Website.Main.Models.Learning;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SetNewOrderViewModel
    {
        public string SourceFolderId { get; set; }
        public BlackMesa.Website.Main.Models.Learning.Folder SourceFolder { get; set; }
        public List<BlackMesa.Website.Main.Models.Learning.IndexCard> Units { get; set; }
    }
}