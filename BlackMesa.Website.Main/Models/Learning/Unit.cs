using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Website.Main.Models.Learning
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
