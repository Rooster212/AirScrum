using Antlr.Runtime;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ScrumTool.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Database;

    internal sealed class Configuration : DbMigrationsConfiguration<ScrumTool.Database.ScrumDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ScrumDB context)
        {
            var roles = new [] { "Admin","ScrumMaster","Developer","ProductOwner" } ;

            foreach (var role in roles)
            {
                if(context.AspNetRoles.Any(r => r.Name == role)) continue; // if the role is already in the database don't add it
                var userRole = new AspNetRole() { Name = role, Id = Guid.NewGuid().ToString() };
                context.AspNetRoles.Add(userRole);
            }
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
