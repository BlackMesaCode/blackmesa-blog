using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Query;
using BlackMesa.Website.Main.Models.Identity;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_Queries")]
    public class Query
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }


        public bool QueryOnlyDueCards { get; set; }
        public bool ReverseSides { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "QueryType")]
        public QueryType QueryType { get; set; }


        public virtual ICollection<Card> CardsToQuery { get; set; }
        public virtual ICollection<QueryItem> QueryItems { get; set; }


        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [NotMapped]
        public TimeSpan Duration { get { return EndTime - StartTime; } }

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
