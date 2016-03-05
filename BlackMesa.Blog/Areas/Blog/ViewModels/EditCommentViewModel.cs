using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlackMesa.Blog.Models.Blog;
using BlackMesa.Blog.Models.Identity;
using BlackMesa.Blog.Resources;

namespace BlackMesa.Blog.Areas.Blog.ViewModels
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