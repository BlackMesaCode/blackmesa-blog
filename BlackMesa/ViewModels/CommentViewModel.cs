using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlackMesa.Models;

namespace BlackMesa.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }

        public int EntryId { get; set; }  // Having both the ForeignKey and the navigation property in place, will make EntryId a not nullable ForeignKey in the database
        public virtual Entry Entry { get; set; }
    }
}