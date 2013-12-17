using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Models.Identity;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_Units")]
    public abstract class Unit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "DateCreated")]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "DateEdited")]
        public DateTime DateEdited { get; set; }

        public Guid FolderId { get; set; }  // Having both the ForeignKey and the navigation property in place, will make FolderId a not nullable ForeignKey in the database
        
        public virtual Folder Entry { get; set; }

    }
}
