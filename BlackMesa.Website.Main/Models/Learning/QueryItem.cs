using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_QueryItems")]
    public class QueryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public QueryResult Result { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public TimeSpan Duration { get { return EndTime - StartTime; } }

        public Guid CardId { get; set; }

        public virtual Card Card { get; set; }

        public Guid QueryId { get; set; }

        public virtual Query Query { get; set; }
    }

    public enum QueryResult
    {
        [Display(Name = "QueryResultCorrect", ResourceType = typeof(Strings))]
        Correct,
        [Display(Name = "QueryResultPartlyCorrect", ResourceType = typeof(Strings))]
        PartlyCorrect,
        [Display(Name = "QueryResultWrong", ResourceType = typeof(Strings))]
        Wrong,
    }
}
