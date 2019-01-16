namespace CloudQuiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "ResultDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "ResultDate");
        }
    }
}
