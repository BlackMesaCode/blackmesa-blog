using System.Data.Entity;
using BlackMesa.Gallery.Model;

namespace BlackMesa.Gallery.DataLayer
{
    public class GalleryDbContext : DbContext
    {
        public GalleryDbContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<Category> GalleryCategories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //configure model with fluent API
        }

    }
}