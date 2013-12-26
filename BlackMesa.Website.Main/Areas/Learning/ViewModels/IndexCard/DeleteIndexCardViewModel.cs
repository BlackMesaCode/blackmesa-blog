using System.ComponentModel.DataAnnotations;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.IndexCard
{
    public class DeleteIndexCardViewModel
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string FolderId { get; set; }

        [StringLength(255)]
        public string Question { get; set; }

    }
}