using BlackMesa.Website.Main.Areas.Learning.ViewModels.LearningUnit;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.IndexCard
{
    public class CreateIndexCardViewModel : CreateLearningUnitViewModel
    {
        public string Question { get; set; }

        public string Answer { get; set; }

    }
}