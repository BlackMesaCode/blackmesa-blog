namespace BlackMesa.Website.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedQuestionAnswerNames : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Learning_IndexCards","Question", "FrontSide");
            RenameColumn("dbo.Learning_IndexCards","Answer", "BackSide");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Learning_IndexCards", "FrontSide", "Question");
            RenameColumn("dbo.Learning_IndexCards", "BackSide", "Answer");

        }
    }
}
