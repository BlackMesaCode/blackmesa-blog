using System.ComponentModel.DataAnnotations;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.IndexCard
{
    public class EditIndexCardViewModel
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string FolderId { get; set; }

        [StringLength(255)]
        public string FrontSide { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string BackSide { get; set; }

        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string Hint { get; set; }

        [StringLength(10000)]
        public string CodeSnipped { get; set; }

        [StringLength(2083)]
        public string ImageUrl { get; set; }



    }
}