using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_IndexCards")]
    public class IndexCard : Unit
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
