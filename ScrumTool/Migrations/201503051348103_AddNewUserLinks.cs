namespace ScrumTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewUserLinks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamAspNetUsers",
                c => new
                    {
                        Team_ID = c.Int(nullable: false),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Team_ID, t.AspNetUser_Id })
                .ForeignKey("dbo.Team", t => t.Team_ID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: true)
                .Index(t => t.Team_ID)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.ProjectAspNetUsers",
                c => new
                    {
                        Project_ID = c.Int(nullable: false),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Project_ID, t.AspNetUser_Id })
                .ForeignKey("dbo.Project", t => t.Project_ID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: true)
                .Index(t => t.Project_ID)
                .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectAspNetUsers", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectAspNetUsers", "Project_ID", "dbo.Project");
            DropForeignKey("dbo.TeamAspNetUsers", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamAspNetUsers", "Team_ID", "dbo.Team");
            DropIndex("dbo.ProjectAspNetUsers", new[] { "AspNetUser_Id" });
            DropIndex("dbo.ProjectAspNetUsers", new[] { "Project_ID" });
            DropIndex("dbo.TeamAspNetUsers", new[] { "AspNetUser_Id" });
            DropIndex("dbo.TeamAspNetUsers", new[] { "Team_ID" });
            DropTable("dbo.ProjectAspNetUsers");
            DropTable("dbo.TeamAspNetUsers");
        }
    }
}
