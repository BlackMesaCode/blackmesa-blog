using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Query
{
    public class QueryViewModel : QueryBaseViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Hint { get; set; }
        public string CodeSnipped { get; set; }
        public string ImageUrl { get; set; }

    }
}