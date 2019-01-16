namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "QuestionImage", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "QuestionImage");
        }
    }
}
