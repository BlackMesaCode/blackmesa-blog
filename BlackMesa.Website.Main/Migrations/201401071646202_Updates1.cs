namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updates1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Learning_Cards", "Hint");
            DropColumn("dbo.Learning_Cards", "CodeSnipped");
            DropColumn("dbo.Learning_Cards", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Learning_Cards", "ImageUrl", c => c.String(maxLength: 2083));
            AddColumn("dbo.Learning_Cards", "CodeSnipped", c => c.String());
            AddColumn("dbo.Learning_Cards", "Hint", c => c.String(maxLength: 255));
        }
    }
}
