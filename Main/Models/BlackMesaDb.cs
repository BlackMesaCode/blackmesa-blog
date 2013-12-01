using System.Data.Entity;

namespace Blog.Main.Models
{
    public class BlackMesaDb : DbContext
    {
        public BlackMesaDb() : base("DefaultConnection")
        {
            
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //configure model with fluent API
        }

    }
}