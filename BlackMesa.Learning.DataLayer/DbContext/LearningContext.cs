using System.Data.Entity;
using BlackMesa.Learning.Model;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BlackMesa.Learning.DataLayer.DbContext
{
    public class LearningContext : System.Data.Entity.DbContext
    {
        public LearningContext() : base("DefaultConnection")
        {
            
        }

        // Blog Entities

        public DbSet<Folder> Folders { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<FreeTextUnit> FreeTextUnits { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUserRole>().ToTable("Identity_UserRoles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("Identity_UserLogins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("Identity_UserClaims");
            //modelBuilder.Entity<IdentityRole>().ToTable("Identity_Roles");

            //modelBuilder.Entity<IdentityUser>().ToTable("Identity_Users");
            //modelBuilder.Entity<User>().ToTable("Identity_Users");
        }

    }
}