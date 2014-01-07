using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Query
{
    public class SetupQueryViewModel
    {
        public string FolderId { get; set; }

        public bool QueryOnlyDueCards { get; set; }
        public bool ReverseSides { get; set; }


        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "QueryType")]
        public QueryType QueryType { get; set; }

    }


    public enum OrderType
    {
        [Display(Name = "OrderTypeOrdered", ResourceType = typeof(Strings))]
        Ordered,
        [Display(Name = "OrderTypeShuffled", ResourceType = typeof(Strings))]
        Shuffled,
    }

    public enum QueryType
    {
        [Display(Name = "QueryTypeNormal", ResourceType = typeof(Strings))]
        Normal,
        [Display(Name = "QueryTypeSinglePass", ResourceType = typeof(Strings))]
        SinglePass,
    }

}