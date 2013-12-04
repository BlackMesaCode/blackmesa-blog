namespace BlackMesa.Blog.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "TagsAsString", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Entries", "TagsAsString");
        }
    }
}
