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

    }
}