namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration8 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Results", new[] { "User_Id" });
            DropColumn("dbo.Results", "UserId");
            RenameColumn(table: "dbo.Results", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Results", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Results", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Results", new[] { "UserId" });
            AlterColumn("dbo.Results", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Results", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Results", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Results", "User_Id");
        }
    }
}
