using System.Data.Entity;
using BlackMesa.Blog.Model;
using BlackMesa.Gallery.Model;

namespace BlackMesa.Website.Main.DataLayer
{
    public class WebsiteDbContext : DbContext
    {
        public WebsiteDbContext() : base("DefaultConnection")
        {
            
        }

        // Blog Entities

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }


        // Gallery Entities

        public DbSet<Category> Categories { get; set; }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Tag>()
        //        .HasMany(x => x.Entries)
        //        .WithMany(x => x.Tags)
        //        .Map(x =>
        //        {
        //            x.ToTable("Blog_TagEntries"); // name of association table
        //            x.MapLeftKey("Tag_Id");
        //            x.MapRightKey("Entry_Id");
        //        });
        //}

    }
}