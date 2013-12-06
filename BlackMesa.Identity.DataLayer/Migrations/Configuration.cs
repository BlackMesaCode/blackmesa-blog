using System.Data.Entity.Migrations;
using BlackMesa.Identity.DataLayer.DbContext;
using BlackMesa.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Identity.DataLayer.Migrations
{

    public sealed class Configuration : DbMigrationsConfiguration<IdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IdentityContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("RegisteredUser"))
                roleManager.Create(new IdentityRole("RegisteredUser"));

            var userManager = new UserManager<User>(new UserStore<User>(context));

 
            var admin = new User
            {
                UserName = "Tester",
            };


            if (userManager.FindByName("sum@live.de") == null)
            {
                userManager.Create(admin, "test123");
                //userManager.AddToRole(admin.Id, "Admin");
            }
            userManager.AddToRole(userManager.FindByName("Tester").Id, "Admin");
            //var adminUser = userManager.FindByName("sum@live.de");
            ////if (adminUser != null)
            //    

        }
    }
}
