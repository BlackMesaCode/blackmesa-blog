namespace BlackMesa.Blog.DataLayer.Migrations
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
                        Name = c.String(maxLength: 255),
                        Email = c.String(maxLength: 254),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        EntryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog_Entries", t => t.EntryId, cascadeDelete: true)
                .Index(t => t.EntryId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blog_TagEntries", "Entry_Id", "dbo.Blog_Entries");
            DropForeignKey("dbo.Blog_TagEntries", "Tag_Id", "dbo.Blog_Tags");
            DropForeignKey("dbo.Blog_Comments", "EntryId", "dbo.Blog_Entries");
            DropIndex("dbo.Blog_TagEntries", new[] { "Entry_Id" });
            DropIndex("dbo.Blog_TagEntries", new[] { "Tag_Id" });
            DropIndex("dbo.Blog_Comments", new[] { "EntryId" });
            DropTable("dbo.Blog_TagEntries");
            DropTable("dbo.Blog_Tags");
            DropTable("dbo.Blog_Entries");
            DropTable("dbo.Blog_Comments");
        }
    }
}
