using Microsoft.AspNet.Identity.EntityFramework;
using ScrumTool.Models;

namespace ScrumTool.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ScrumDB : DbContext
    {
        public ScrumDB()
            : base("name=ScrumDB")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BacklogItem> BacklogItems { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<ScrumTask> Tasks { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<BacklogItem>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BacklogItem>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<BacklogItem>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.BacklogItem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BacklogItem>()
                .HasMany(e => e.Sprints)
                .WithMany(e => e.BacklogItems)
                .Map(m => m.ToTable("SprintBacklogItem").MapLeftKey("BacklogItemID").MapRightKey("SprintID"));

            modelBuilder.Entity<Project>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.BacklogItems)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Sprints)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Teams)
                .WithMany(e => e.Projects)
                .Map(m => m.ToTable("TeamProjects").MapLeftKey("ProjectID").MapRightKey("TeamID"));

            modelBuilder.Entity<Sprint>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ScrumTask>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ScrumTask>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.Name)
                .IsUnicode(false);
        
        }
    }
}
