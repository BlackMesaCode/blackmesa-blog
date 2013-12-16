using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels
{
    public class CreateEditFolderViewModel
    {

        public int Id { get; set; } // todo change int Id to Guid Id

        public int? ParentFolderId { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

    }
}