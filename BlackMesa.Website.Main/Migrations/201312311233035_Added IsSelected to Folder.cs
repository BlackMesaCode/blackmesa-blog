namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsSelectedtoFolder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Learning_Folders", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Learning_Folders", "IsSelected");
        }
    }
}
