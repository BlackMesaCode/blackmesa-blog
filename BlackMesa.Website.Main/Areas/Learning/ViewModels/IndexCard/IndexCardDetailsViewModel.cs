using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.IndexCard
{
    public class IndexCardDetailsViewModel
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string FolderId { get; set; }

        public Dictionary<string, string> Path { get; set; }

        [StringLength(255)]
        public string Question { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; }

        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string Hint { get; set; }

        [StringLength(10000)]
        public string CodeSnipped { get; set; }

        [StringLength(2083)]
        public string ImageUrl { get; set; }

        public List<BlackMesa.Website.Main.Models.Learning.QueryItem> Queries { get; set; }
    }

}