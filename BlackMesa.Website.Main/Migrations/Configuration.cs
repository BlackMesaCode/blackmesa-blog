using BlackMesa.Website.Main.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlackMesa.Website.Main.DataLayer.BlackMesaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlackMesa.Website.Main.DataLayer.BlackMesaDbContext context)
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


            if (userManager.FindByName("Tester") == null)
                userManager.Create(admin, "test123");

            if (!userManager.IsInRole(admin.Id, "Admin"))
                userManager.AddToRole(admin.Id, "Admin");


        }
    }
}
