using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackMesa.Identity.Model;

namespace BlackMesa.Learning.Model
{
    [Table("Learning_Folders")]
    public class Folder
    {

        public Folder(int level)
        {
            Level = level;
        }

        public int Id { get; set; }

        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Level { get; set; }

        public virtual ICollection<Folder> SubFolders { get; set; }
        public virtual ICollection<Unit> LearningUnits { get; set; }

    }
}
