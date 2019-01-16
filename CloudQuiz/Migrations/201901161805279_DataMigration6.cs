namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "QuestionImage", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "QuestionImage", c => c.Binary(nullable: false));
        }
    }
}
