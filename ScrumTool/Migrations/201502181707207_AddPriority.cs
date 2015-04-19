namespace ScrumTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriority : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.Id);
            
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
                "dbo.BacklogItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 500, unicode: false),
                        Comments = c.String(unicode: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Sprint",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500, unicode: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ScrumTasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500, unicode: false),
                        Weight = c.Int(),
                        Comments = c.String(unicode: false),
                        BacklogItemID = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BacklogItem", t => t.BacklogItemID)
                .Index(t => t.BacklogItemID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TeamProjects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false),
                        TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.TeamID })
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Team", t => t.TeamID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.TeamID);
            
            CreateTable(
                "dbo.SprintBacklogItem",
                c => new
                    {
                        BacklogItemID = c.Int(nullable: false),
                        SprintID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BacklogItemID, t.SprintID })
                .ForeignKey("dbo.BacklogItem", t => t.BacklogItemID, cascadeDelete: true)
                .ForeignKey("dbo.Sprint", t => t.SprintID, cascadeDelete: true)
                .Index(t => t.BacklogItemID)
                .Index(t => t.SprintID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScrumTasks", "BacklogItemID", "dbo.BacklogItem");
            DropForeignKey("dbo.SprintBacklogItem", "SprintID", "dbo.Sprint");
            DropForeignKey("dbo.SprintBacklogItem", "BacklogItemID", "dbo.BacklogItem");
            DropForeignKey("dbo.TeamProjects", "TeamID", "dbo.Team");
            DropForeignKey("dbo.TeamProjects", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Sprint", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.BacklogItem", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SprintBacklogItem", new[] { "SprintID" });
            DropIndex("dbo.SprintBacklogItem", new[] { "BacklogItemID" });
            DropIndex("dbo.TeamProjects", new[] { "TeamID" });
            DropIndex("dbo.TeamProjects", new[] { "ProjectID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.ScrumTasks", new[] { "BacklogItemID" });
            DropIndex("dbo.Sprint", new[] { "ProjectID" });
            DropIndex("dbo.BacklogItem", new[] { "ProjectID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropTable("dbo.SprintBacklogItem");
            DropTable("dbo.TeamProjects");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ScrumTasks");
            DropTable("dbo.Team");
            DropTable("dbo.Sprint");
            DropTable("dbo.Project");
            DropTable("dbo.BacklogItem");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}
