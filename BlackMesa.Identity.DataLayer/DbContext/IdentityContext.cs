using System.Data.Entity;
using BlackMesa.Identity.DataLayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Identity.DataLayer.DbContext
{

    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext()
            : base("DefaultConnection")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserRole>().ToTable("Identity_UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Identity_UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Identity_UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Identity_Roles");

            modelBuilder.Entity<IdentityUser>().ToTable("Identity_Users");
            modelBuilder.Entity<User>().ToTable("Identity_Users");

        }

    }


}