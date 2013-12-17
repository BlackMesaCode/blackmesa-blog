using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Models.Learning;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels
{
    public class FolderIndexViewModel
    {

        public int TotalLearningUnits { get; set; } 
        public List<FolderListItemViewModel> Folders { get; set; } 

    }
}