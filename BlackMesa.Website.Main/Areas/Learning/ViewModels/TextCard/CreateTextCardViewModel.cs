using BlackMesa.Website.Main.Areas.Learning.ViewModels.LearningUnit;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.TextCard
{
    public class CreateTextCardViewModel : CreateLearningUnitViewModel
    {
        public string Question { get; set; }

        public string Answer { get; set; }

    }
}