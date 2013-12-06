namespace BlackMesa.Identity.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Identity_Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Identity_Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Identity_UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Identity_Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Identity_UserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.Identity_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Identity_UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Identity_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Identity_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Identity_UserClaims", "User_Id", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserRoles", "UserId", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserRoles", "RoleId", "dbo.Identity_Roles");
            DropForeignKey("dbo.Identity_UserLogins", "UserId", "dbo.Identity_Users");
            DropIndex("dbo.Identity_UserClaims", new[] { "User_Id" });
            DropIndex("dbo.Identity_UserRoles", new[] { "UserId" });
            DropIndex("dbo.Identity_UserRoles", new[] { "RoleId" });
            DropIndex("dbo.Identity_UserLogins", new[] { "UserId" });
            DropTable("dbo.Identity_UserRoles");
            DropTable("dbo.Identity_UserLogins");
            DropTable("dbo.Identity_UserClaims");
            DropTable("dbo.Identity_Users");
            DropTable("dbo.Identity_Roles");
        }
    }
}
