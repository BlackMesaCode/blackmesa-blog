using System.Data.Entity;
using BlackMesa.Blog.Models.Blog;
using BlackMesa.Blog.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Blog.DataLayer
{
    public class BlackMesaDbContext : IdentityDbContext<User>
    {
        public BlackMesaDbContext() : base("DefaultConnection")
        {
            
        }

        // Blog Entities

        public DbSet<Entry> Blog_Entries { get; set; }
        public DbSet<Tag> Blog_Tags { get; set; }
        public DbSet<Comment> Blog_Comments { get; set; }


        // Identity Entities

        // DbSets for Identity come with the inherited IdentityDbContext<User>


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Blog

            modelBuilder.Entity<Tag>()
                .HasMany(x => x.Entries)
                .WithMany(x => x.Tags)
                .Map(x =>
                {
                    x.ToTable("Blog_TagEntries"); // name of association table
                    x.MapLeftKey("Tag_Id");
                    x.MapRightKey("Entry_Id");
                });


            // Identity

            modelBuilder.Entity<IdentityUserRole>().ToTable("Identity_UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Identity_UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Identity_UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Identity_Roles");

            modelBuilder.Entity<IdentityUser>().ToTable("Identity_Users");
            modelBuilder.Entity<User>().ToTable("Identity_Users");


        }

    }
}