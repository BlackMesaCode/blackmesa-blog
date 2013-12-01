using System.Data.Entity.Migrations;

namespace BlackMesa.Blog.Main.Migrations
{
    public partial class Initialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Preview = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Language = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Name = c.String(maxLength: 255),
                        Email = c.String(maxLength: 254),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        EntryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entries", t => t.EntryId, cascadeDelete: true)
                .Index(t => t.EntryId);
            
            CreateTable(
                "dbo.TagEntries",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Entry_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Entry_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Entries", t => t.Entry_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Entry_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TagEntries", new[] { "Entry_Id" });
            DropIndex("dbo.TagEntries", new[] { "Tag_Id" });
            DropIndex("dbo.Comments", new[] { "EntryId" });
            DropForeignKey("dbo.TagEntries", "Entry_Id", "dbo.Entries");
            DropForeignKey("dbo.TagEntries", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Comments", "EntryId", "dbo.Entries");
            DropTable("dbo.TagEntries");
            DropTable("dbo.Comments");
            DropTable("dbo.Tags");
            DropTable("dbo.Entries");
        }
    }
}
