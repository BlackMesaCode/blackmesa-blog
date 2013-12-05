using System.Data.Entity;
using BlackMesa.Blog.Model;

namespace BlackMesa.Blog.DataLayer
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<Entry> BlogEntries { get; set; }
        public DbSet<Tag> BlogTags { get; set; }
        public DbSet<Comment> BlogComments { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //configure model with fluent API
        }

    }
}