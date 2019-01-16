namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quizs", "QuizDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quizs", "QuizDescription");
        }
    }
}
