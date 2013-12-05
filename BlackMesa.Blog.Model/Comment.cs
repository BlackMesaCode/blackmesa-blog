using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Blog.Model
{
    [Table("Blog_Comments")]
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [DefaultValue("Anon")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(254)]
        public string Email { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateEdited { get; set; }

        public int EntryId { get; set; }  // Having both the ForeignKey and the navigation property in place, will make EntryId a not nullable ForeignKey in the database
        public virtual Entry Entry { get; set; }
    }
}