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
        public string SelectedFolderId { get; set; }
        public string SelectedFolderName { get; set; }
        public bool InludeSubfolders { get; set; }
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
        [Display(Name = "OrderTypeReversed", ResourceType = typeof(Strings))]
        Reversed,
        [Display(Name = "OrderTypeShuffled", ResourceType = typeof(Strings))]
        Shuffled,
    }

    public enum QueryType
    {
        [Display(Name = "QueryTypeNormal", ResourceType = typeof(Strings))]
        Normal,
        [Display(Name = "QueryTypeSinglePass", ResourceType = typeof(Strings))]
        SinglePass,
        [Display(Name = "QueryTypeLeitner", ResourceType = typeof(Strings))]
        Leitner,
    }
}