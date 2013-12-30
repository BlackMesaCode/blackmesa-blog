using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_Queries")]
    public class Query
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public QueryResult Result { get; set; }

        [Required]
        public DateTime QuestionTime { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; }

        [NotMapped]
        public TimeSpan AnswerDuration { get { return AnswerTime - QuestionTime; } }

        public Guid UnitId { get; set; }

        public virtual Unit Unit { get; set; }

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
