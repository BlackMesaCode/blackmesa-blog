using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMesa.Learning.Model
{
    [Table("Learning_FreeTextUnits")]
    public class FreeTextUnit : Unit
    {
        public string Question { get; set; }

        public string Answer { get; set; }

    }
}
