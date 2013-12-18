using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Models.Identity;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Blog
{
    [Table("Blog_Comments")]
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Strings), Name = "Content")]
        [StringLength(10000)]
        public string Content { get; set; }

        [Required]
        [StringLength(30)]
        //[DefaultValue(typeof(Strings), "Guest")]
        [Display(ResourceType = typeof(Strings), Name = "Name")]
        public string Name { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateEdited { get; set; }

        [Required]
        public int EntryId { get; set; }  // Having both the ForeignKey and the navigation property in place, will make EntryId a not nullable ForeignKey in the database
        public virtual Entry Entry { get; set; }
    }
}