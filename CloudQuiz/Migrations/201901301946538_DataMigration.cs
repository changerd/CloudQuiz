namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        ChoiceId = c.Int(nullable: false, identity: true),
                        ChoiceText = c.String(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChoiceId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false),
                        QuestionAnswer = c.String(),
                        QuestionDescription = c.String(),
                        QuestionImage = c.Binary(),
                        QuizId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizId = c.Int(nullable: false, identity: true),
                        QuizName = c.String(nullable: false),
                        QuizDuration = c.Time(nullable: false, precision: 7),
                        QuizDescription = c.String(),
                    })
                .PrimaryKey(t => t.QuizId);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        ResultScore = c.Int(nullable: false),
                        ResultDate = c.DateTime(nullable: false),
                        QuizId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ResultId)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.QuizId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Birth = c.DateTime(),
                        Group = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserQuizs",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Quiz_QuizId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Quiz_QuizId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.Quiz_QuizId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Quiz_QuizId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Results", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserQuizs", "Quiz_QuizId", "dbo.Quizs");
            DropForeignKey("dbo.ApplicationUserQuizs", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Results", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Choices", "QuestionId", "dbo.Questions");
            DropIndex("dbo.ApplicationUserQuizs", new[] { "Quiz_QuizId" });
            DropIndex("dbo.ApplicationUserQuizs", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Results", new[] { "UserId" });
            DropIndex("dbo.Results", new[] { "QuizId" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropIndex("dbo.Choices", new[] { "QuestionId" });
            DropTable("dbo.ApplicationUserQuizs");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Results");
            DropTable("dbo.Quizs");
            DropTable("dbo.Questions");
            DropTable("dbo.Choices");
        }
    }
}
