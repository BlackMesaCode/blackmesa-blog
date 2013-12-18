using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Models.Blog;
using BlackMesa.Website.Main.Models.Identity;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Areas.Blog.ViewModels
{
    public class EditCommentViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Strings), Name = "YourComment")]
        [StringLength(10000)]
        public string Content { get; set; }

        [Required]
        public int EntryId { get; set; }

    }
}