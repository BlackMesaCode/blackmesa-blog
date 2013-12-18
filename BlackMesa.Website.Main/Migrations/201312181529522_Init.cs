namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blog_Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        OwnerId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        EntryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog_Entries", t => t.EntryId, cascadeDelete: true)
                .ForeignKey("dbo.Identity_Users", t => t.OwnerId)
                .Index(t => t.EntryId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Blog_Entries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(nullable: false),
                        Prio = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        TagsAsString = c.String(nullable: false),
                        Preview = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blog_Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Language = c.String(nullable: false),
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
            
            CreateTable(
                "dbo.Identity_Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Learning_Folders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OwnerId = c.String(maxLength: 128),
                        Name = c.String(nullable: false),
                        Level = c.Int(nullable: false),
                        ParentFolder_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Identity_Users", t => t.OwnerId)
                .ForeignKey("dbo.Learning_Folders", t => t.ParentFolder_Id)
                .Index(t => t.OwnerId)
                .Index(t => t.ParentFolder_Id);
            
            CreateTable(
                "dbo.Learning_Units",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OwnerId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        FolderId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Learning_Folders", t => t.FolderId, cascadeDelete: true)
                .ForeignKey("dbo.Identity_Users", t => t.OwnerId)
                .Index(t => t.FolderId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Blog_TagEntries",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Entry_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Entry_Id })
                .ForeignKey("dbo.Blog_Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Blog_Entries", t => t.Entry_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Entry_Id);
            
            CreateTable(
                "dbo.Learning_TextCards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Learning_Units", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Learning_TextCards", "Id", "dbo.Learning_Units");
            DropForeignKey("dbo.Learning_Folders", "ParentFolder_Id", "dbo.Learning_Folders");
            DropForeignKey("dbo.Learning_Folders", "OwnerId", "dbo.Identity_Users");
            DropForeignKey("dbo.Learning_Units", "OwnerId", "dbo.Identity_Users");
            DropForeignKey("dbo.Learning_Units", "FolderId", "dbo.Learning_Folders");
            DropForeignKey("dbo.Blog_Comments", "OwnerId", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserClaims", "User_Id", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserRoles", "UserId", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserRoles", "RoleId", "dbo.Identity_Roles");
            DropForeignKey("dbo.Identity_UserLogins", "UserId", "dbo.Identity_Users");
            DropForeignKey("dbo.Blog_TagEntries", "Entry_Id", "dbo.Blog_Entries");
            DropForeignKey("dbo.Blog_TagEntries", "Tag_Id", "dbo.Blog_Tags");
            DropForeignKey("dbo.Blog_Comments", "EntryId", "dbo.Blog_Entries");
            DropIndex("dbo.Learning_TextCards", new[] { "Id" });
            DropIndex("dbo.Learning_Folders", new[] { "ParentFolder_Id" });
            DropIndex("dbo.Learning_Folders", new[] { "OwnerId" });
            DropIndex("dbo.Learning_Units", new[] { "OwnerId" });
            DropIndex("dbo.Learning_Units", new[] { "FolderId" });
            DropIndex("dbo.Blog_Comments", new[] { "OwnerId" });
            DropIndex("dbo.Identity_UserClaims", new[] { "User_Id" });
            DropIndex("dbo.Identity_UserRoles", new[] { "UserId" });
            DropIndex("dbo.Identity_UserRoles", new[] { "RoleId" });
            DropIndex("dbo.Identity_UserLogins", new[] { "UserId" });
            DropIndex("dbo.Blog_TagEntries", new[] { "Entry_Id" });
            DropIndex("dbo.Blog_TagEntries", new[] { "Tag_Id" });
            DropIndex("dbo.Blog_Comments", new[] { "EntryId" });
            DropTable("dbo.Learning_TextCards");
            DropTable("dbo.Blog_TagEntries");
            DropTable("dbo.Learning_Units");
            DropTable("dbo.Learning_Folders");
            DropTable("dbo.Identity_Roles");
            DropTable("dbo.Identity_UserRoles");
            DropTable("dbo.Identity_UserLogins");
            DropTable("dbo.Identity_UserClaims");
            DropTable("dbo.Identity_Users");
            DropTable("dbo.Blog_Tags");
            DropTable("dbo.Blog_Entries");
            DropTable("dbo.Blog_Comments");
        }
    }
}
