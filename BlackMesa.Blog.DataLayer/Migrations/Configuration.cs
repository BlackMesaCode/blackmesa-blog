using System.Data.Entity.Migrations;

namespace BlackMesa.Blog.DataLayer.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<BlogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlogDbContext context)
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

            //AuthConfig.RegisterAuth();

            //var roles = (SimpleRoleProvider)Roles.Provider;
            //var membership = (SimpleMembershipProvider)Membership.Provider;

            //if (!Roles.RoleExists("Admin"))
            //    roles.CreateRole("Admin");

            //if (!Roles.RoleExists("RegisteredUser"))
            //    roles.CreateRole("RegisteredUser");

            //if (membership.GetUser("sum@live.de", false) == null)
            //    membership.CreateUserAndAccount("sum@live.de", "r3volTec");

            //if (membership.GetUser("Tester", false) == null)
            //    membership.CreateUserAndAccount("Tester", "go4testing");

            //if (!new List<string>(roles.GetRolesForUser("sum@live.de")).Contains("Admin"))
            //    roles.AddUsersToRoles(new[] { "sum@live.de" }, new[] { "Admin" });

            //if (!new List<string>(roles.GetRolesForUser("Tester")).Contains("Admin"))
            //    roles.AddUsersToRoles(new[] { "Tester" }, new[] { "Admin" });
        }
    }
}
