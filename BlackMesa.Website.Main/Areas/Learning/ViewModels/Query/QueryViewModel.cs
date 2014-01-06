using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Models.Learning;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Query
{
    public class QueryViewModel
    {
        public string FolderId { get; set; }
        public string SelectedCards { get; set; }
        public string RemainingCards { get; set; }
        public DateTime StartTime { get; set; }
        public int Position { get; set; }
        public QueryType QueryType { get; set; }
        public bool ReverseSides { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public string Hint { get; set; }
        public string CodeSnipped { get; set; }
        public string ImageUrl { get; set; }


        public QueryResult Result { get; set; }

    }
}