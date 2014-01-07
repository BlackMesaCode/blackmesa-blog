﻿using System.ComponentModel.DataAnnotations;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Card
{
    public class EditCardViewModel
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string FolderId { get; set; }

        [StringLength(255)]
        public string FrontSide { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string BackSide { get; set; }

    }
}