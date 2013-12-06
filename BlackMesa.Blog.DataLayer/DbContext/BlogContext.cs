using System.Data.Entity;
using BlackMesa.Blog.DataLayer.Models;

namespace BlackMesa.Blog.DataLayer.DbContext
{
    public class BlogContext : System.Data.Entity.DbContext
    {
        public BlogContext() : base("DefaultConnection")
        {
            
        }

        // Blog Entities

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasMany(x => x.Entries)
                .WithMany(x => x.Tags)
                .Map(x =>
                {
                    x.ToTable("Blog_TagEntries"); // name of association table
                    x.MapLeftKey("Tag_Id");
                    x.MapRightKey("Entry_Id");
                });
        }

    }
}