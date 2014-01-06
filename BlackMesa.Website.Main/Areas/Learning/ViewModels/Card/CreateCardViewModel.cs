using System.ComponentModel.DataAnnotations;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Card;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Card
{
    public class CreateCardViewModel
    {
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