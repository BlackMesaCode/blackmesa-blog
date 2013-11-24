using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlackMesa.Blog.Models
{
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