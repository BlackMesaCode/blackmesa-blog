using System.ComponentModel.DataAnnotations;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder
{
    public class CreateViewModel
    {
        public string Id { get; set; }

        public string ParentFolderId { get; set; }

        [Required]
        public string Name { get; set; }


    }
}