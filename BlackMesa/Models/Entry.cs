using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackMesa.Models
{
    //    [ValidateInput(false)]  // disables request validation on complete model or view model
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
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