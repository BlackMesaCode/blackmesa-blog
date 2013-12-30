using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Models.Learning;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Query
{
    public class QueryCompletedViewModel
    {
        public string FolderId { get; set; }
        public int TotalCount { get; set; }
        public int CorrectCount { get; set; }
        public int PartlyCorrectCount { get; set; }
        public int WrongCount { get; set; }

    }
}