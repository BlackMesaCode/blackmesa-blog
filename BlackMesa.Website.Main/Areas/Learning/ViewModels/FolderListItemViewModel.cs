using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels
{
    public class FolderListItemViewModel
    {
        public int Id { get; set; }

        public int? ParentFolderId { get; set; }
        
        public string Name { get; set; }

        public int Level { get; set; }

        public List<FolderListItemViewModel> SubFolders { get; set; }

    }
}