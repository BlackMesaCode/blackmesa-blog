using System.ComponentModel.DataAnnotations.Schema;

namespace BlackMesa.Gallery.Model
{
    [Table("Gallery_Categories")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}