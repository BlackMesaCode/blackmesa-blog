namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Learning_QueryItems", "CardId", "dbo.Learning_Cards");
            DropIndex("dbo.Learning_QueryItems", new[] { "CardId" });
            CreateTable(
                "dbo.Learning_TestItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Result = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CardId = c.Guid(nullable: false),
                        TestId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Learning_Cards", t => t.CardId, cascadeDelete: true)
                .Index(t => t.CardId);
            
            DropTable("dbo.Learning_QueryItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Learning_QueryItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Result = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CardId = c.Guid(nullable: false),
                        QueryId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Learning_TestItems", "CardId", "dbo.Learning_Cards");
            DropIndex("dbo.Learning_TestItems", new[] { "CardId" });
            DropTable("dbo.Learning_TestItems");
            CreateIndex("dbo.Learning_QueryItems", "CardId");
            AddForeignKey("dbo.Learning_QueryItems", "CardId", "dbo.Learning_Cards", "Id", cascadeDelete: true);
        }
    }
}
