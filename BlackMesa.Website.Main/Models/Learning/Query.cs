using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Query;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_Queries")]
    public class Query
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool InludeSubfolders { get; set; }
        public bool ReverseSides { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "QueryType")]
        public QueryType QueryType { get; set; }


        public string SelectedCards { get; set; }
        public string RemainingCards { get; set; }


        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public TimeSpan BackSideDuration { get { return EndTime - StartTime; } }

        public Guid CardId { get; set; }

        public virtual Card Card { get; set; }

    }

}
