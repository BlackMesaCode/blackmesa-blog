using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMesa.Learning.Model
{
    [Table("Learning_Units")]
    public abstract class Unit
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Guid Id { get; set; }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public int FolderId { get; set; }  // Having both the ForeignKey and the navigation property in place, will make FolderId a not nullable ForeignKey in the database
        public virtual Folder Entry { get; set; }
    }
}
