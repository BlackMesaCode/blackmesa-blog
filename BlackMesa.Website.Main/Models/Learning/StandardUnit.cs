using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_StandardUnits")]
    public class StandardUnit : Unit
    {
        public string Question { get; set; }

        public string Answer { get; set; }

    }
}
