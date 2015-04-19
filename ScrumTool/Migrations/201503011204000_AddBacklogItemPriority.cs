namespace ScrumTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBacklogItemPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BacklogItem", "Priority", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BacklogItem", "Priority");
        }
    }
}
