namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsSelected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Learning_Units", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Learning_Units", "IsSelected");
        }
    }
}
