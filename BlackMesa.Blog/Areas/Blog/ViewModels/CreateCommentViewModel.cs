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
    public class CreateCommentViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(ResourceType = typeof(Strings), Name = "YourName")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(10000)]
        [Display(ResourceType = typeof(Strings), Name = "YourComment")]
        public string Content { get; set; }

        [Required]
        public int EntryId { get; set; }

    }
}