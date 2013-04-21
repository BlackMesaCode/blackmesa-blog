using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlackMesa.Models
{
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateEdited { get; set; }


         // Any ICollections tagged as 'virtual' will be lazy-loaded unless you specifically mark them otherwise.
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


//        public UserProfile Author { get; set; }
//        public EntryType Type { get; set; }

    }
}