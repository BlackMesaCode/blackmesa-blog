using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Models.Learning;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Test
{
    public class TestCompletedViewModel
    {
        public string FolderId { get; set; }
        public int TotalCount { get; set; }
        public int CorrectCount { get; set; }
        public int PartlyCorrectCount { get; set; }
        public int WrongCount { get; set; }
        public TimeSpan Duration { get; set; }

    }
}