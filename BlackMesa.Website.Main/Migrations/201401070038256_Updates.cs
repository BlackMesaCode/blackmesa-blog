namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Learning_Cards", "FrontSide", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Learning_Cards", "FrontSide", c => c.String(maxLength: 255));
        }
    }
}
