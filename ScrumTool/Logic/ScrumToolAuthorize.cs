using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumTool.Database;

namespace ScrumTool.Logic
{
    public class ProjectAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var auth = base.AuthorizeCore(httpContext);
            if (!auth)
            {
                // user is not authenticated
                return false;
            }

            var user = httpContext.User;

            var routeData = httpContext.Request.RequestContext.RouteData;

            var projId = int.Parse(routeData.Values["projId"].ToString());

            return IsAllowedToAccessProject(user.Identity.Name, projId);
        }

        private bool IsAllowedToAccessProject(string username, int id)
        {
            using (var entities = new ScrumDB())
            {
                var proj = entities.Projects.Find(id);
                return proj.Users.Any(p => p.UserName == username); // if the username is in the project's username list
            }
        }
    }

    public class TeamAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var auth = base.AuthorizeCore(httpContext);
            if (!auth)
            {
                // user is not authenticated
                return false;
            }

            var user = httpContext.User;

            var routeData = httpContext.Request.RequestContext.RouteData;

            int teamId = int.Parse(routeData.Values["teamId"].ToString());
            return UserInTeam(user.Identity.Name, teamId);
        }

        private bool UserInTeam(string username, int teamId)
        {
            using (var entities = new ScrumDB())
            {
                var team = entities.Teams.Find(teamId);
                return team.Users.Any(p => p.UserName == username); // if the username is in the teams list
            }
        }
    }

    public static class ScrumToolAuthorize
    {
        public static List<Project> AllowedProjects(string username)
        {
            using (var entities = new ScrumDB())
            {
                // lookup valid projects (teams and direct users)
                var projects = entities.Projects.Where(p => p.Users.Any(u => u.UserName == username)
                    || p.Teams.Any(t => t.Users.Any(u => u.UserName == username))).Include("BacklogItems")
                            .Include("BacklogItems.Tasks")
                            .Include("Sprints").ToList();

                return projects;
            }
        }
    }
}