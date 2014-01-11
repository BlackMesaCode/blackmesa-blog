using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Models.Identity;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_Folders")]
    public class Folder
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Level { get; set; }

        public bool IsSelected { get; set; }

        [NotMapped]
        public bool IsRootFolder {
            get
            {
                return (ParentFolder == null);
            } 
        }

        public virtual Folder ParentFolder { get; set; }
        public virtual List<Folder> SubFolders { get; set; }
        public virtual List<Card> Cards { get; set; }

    }
}
