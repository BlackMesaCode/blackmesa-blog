using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_TextCards")]
    public class TextCard : Unit
    {
        public string Question { get; set; }

        public string Answer { get; set; }

    }
}
