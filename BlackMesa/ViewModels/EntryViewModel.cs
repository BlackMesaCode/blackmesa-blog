using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Models;

namespace BlackMesa.ViewModels
{
    public class EntryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string TagsString { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateEdited { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}