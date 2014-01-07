﻿using System.Collections.Generic;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection
{
    public class SelectionOptionsViewModel
    {
        public string Id { get; set; }
        public BlackMesa.Website.Main.Models.Learning.Folder Folder { get; set; }
        public bool HasAnyFolderSelected { get; set; }
        public bool HasRootFolderSelected { get; set; }
        public bool HasOnlyCardsSelected { get; set; }
        public bool HasOnlyOneCardSelected { get; set; }
        public string CardId { get; set; }
        public bool HasOnlyFoldersSelected { get; set; }
        public bool HasOnlyOneFolderSelected { get; set; }
        public string FolderId { get; set; }
        
    }
}