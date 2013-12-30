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
        public string SelectedLearningUnits { get; set; }
        public string RemainingLearningUnits { get; set; }
        public DateTime QuestionTime { get; set; }
        public int Position { get; set; }
        public QueryType QueryType { get; set; }


        public string Question { get; set; }
        public string Answer { get; set; }
        public string Hint { get; set; }
        public string CodeSnipped { get; set; }
        public string ImageUrl { get; set; }


        public QueryResult Result { get; set; }

    }
}