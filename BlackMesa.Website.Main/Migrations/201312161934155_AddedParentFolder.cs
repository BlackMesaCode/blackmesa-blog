namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedParentFolder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Learning_Folders", "Folder_Id", "dbo.Learning_Folders");
            DropIndex("dbo.Learning_Folders", new[] { "Folder_Id" });
            DropColumn("dbo.Learning_Folders", "OwnerId");
            RenameColumn(table: "dbo.Learning_Folders", name: "Owner_Id", newName: "OwnerId");
            AddColumn("dbo.Learning_Folders", "ParentFolder_Id", c => c.Int());
            AlterColumn("dbo.Learning_Folders", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Learning_Folders", "ParentFolder_Id");
            AddForeignKey("dbo.Learning_Folders", "ParentFolder_Id", "dbo.Learning_Folders", "Id");
            DropColumn("dbo.Learning_Folders", "Folder_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Learning_Folders", "Folder_Id", c => c.Int());
            DropForeignKey("dbo.Learning_Folders", "ParentFolder_Id", "dbo.Learning_Folders");
            DropIndex("dbo.Learning_Folders", new[] { "ParentFolder_Id" });
            AlterColumn("dbo.Learning_Folders", "OwnerId", c => c.Int(nullable: false));
            DropColumn("dbo.Learning_Folders", "ParentFolder_Id");
            RenameColumn(table: "dbo.Learning_Folders", name: "OwnerId", newName: "Owner_Id");
            AddColumn("dbo.Learning_Folders", "OwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Learning_Folders", "Folder_Id");
            AddForeignKey("dbo.Learning_Folders", "Folder_Id", "dbo.Learning_Folders", "Id");
        }
    }
}
