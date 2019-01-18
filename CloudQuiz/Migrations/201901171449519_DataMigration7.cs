namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            AddColumn("dbo.Questions", "QuestionAnswer", c => c.String());
            DropTable("dbo.Answers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        AnswerText = c.String(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId);
            
            DropColumn("dbo.Questions", "QuestionAnswer");
            CreateIndex("dbo.Answers", "QuestionId");
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "QuestionId", cascadeDelete: true);
        }
    }
}
