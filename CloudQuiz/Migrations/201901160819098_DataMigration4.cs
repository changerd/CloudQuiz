namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Answers", "AnswerText", c => c.String(nullable: false));
            AlterColumn("dbo.Questions", "QuestionText", c => c.String(nullable: false));
            AlterColumn("dbo.Choices", "ChoiceText", c => c.String(nullable: false));
            AlterColumn("dbo.Quizs", "QuizName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quizs", "QuizName", c => c.String());
            AlterColumn("dbo.Choices", "ChoiceText", c => c.String());
            AlterColumn("dbo.Questions", "QuestionText", c => c.String());
            AlterColumn("dbo.Answers", "AnswerText", c => c.String());
        }
    }
}
