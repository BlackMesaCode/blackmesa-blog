using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Resources;

namespace BlackMesa.Blog.Model
{
    [Table("Blog_Entries")]
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public string Language { get; set; }

        public int Prio { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "Tags")]
        public string TagsAsString { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Strings), Name = "Preview")]
        public string Preview { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Strings), Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "DateCreated")]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "DateEdited")]
        public DateTime DateEdited { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "Published")]
        public bool Published { get; set; }

         // Any ICollections tagged as 'virtual' will be lazy-loaded unless you specifically mark them otherwise.
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


//        public UserProfile Author { get; set; }
//        public EntryType Type { get; set; }

    }
}