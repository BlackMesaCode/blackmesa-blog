using System.ComponentModel.DataAnnotations;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.LearningUnit;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.IndexCard
{
    public class CreateIndexCardViewModel : CreateLearningUnitViewModel
    {

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