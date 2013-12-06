using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Blog.DataLayer.Models
{
    [Table("Blog_Tags")]
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }
    }
}