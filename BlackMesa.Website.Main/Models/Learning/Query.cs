using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Query;
using BlackMesa.Website.Main.Models.Identity;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Serializable]
    public class Query
    {
        public Guid Id { get; set; }

        public bool QueryOnlyDueCards { get; set; }
        public bool ReverseSides { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "QueryType")]
        public QueryType QueryType { get; set; }

        public List<Card> CardsToQuery { get; set; }

        public QueryStatus QueryStatus { get; set; }

    }

    public enum QueryStatus
    {
        InProgress,
        Paused,
        Completed,
        Aborted,
    }

}
