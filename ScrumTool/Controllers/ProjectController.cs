using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ScrumTool.Database;
using ScrumTool.Objects;

namespace ScrumTool.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        // GET: Project
        public ActionResult Index()
        {
            using (var entities = new ScrumDB())
            {
                try
                {
                    return View(entities.Projects.ToList());
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
                
            }
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            //TODO: add error handling here
            using (var entities = new ScrumDB())
            {
                var proj = entities.Projects.Include("Teams").Include("Users").FirstOrDefault(p => p.ID == id);
                if (proj == null)
                {
                    ViewBag.ErrorMessage = string.Format("Project id {0} was not found.", id);
                    return RedirectToAction("Index");
                }
                var allBacklogItems = entities.BacklogItems.Where(b => b.ProjectID == id);
                ViewBag.BacklogItems = allBacklogItems;

                // TEAM MEMBERS
                var teamMembers = new Dictionary<int, List<string>>();

                var teams = entities.Projects.Find(id).Teams;

                foreach (var team in teams)
                {
                    var userList = team.Users.Select(user => user.UserName).ToList();

                    teamMembers.Add(team.ID, userList);
                }
                ViewBag.TeamMembers = teamMembers;

                // USER ROLES
                var teamUsers = new Dictionary<string, List<string>>();
                var users = entities.Projects.Find(id).Users;

                foreach (var user in users)
                {
                    var roleList = user.AspNetRoles.Select(role => role.Name).ToList();
                    teamUsers.Add(user.Id,roleList);
                }
                ViewBag.TeamMemberRoles = teamUsers;

                return View(proj);
            }
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    entities.Projects.Add(new Project()
                    {
                        Name = collection["Name"]
                    });
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            //TODO: add error handling here
            using (var entities = new ScrumDB())
            {
                var proj = entities.Projects.Find(id);
                if (proj == null)
                {
                    ViewBag.ErrorMessage = string.Format("Project id {0} was not found.", id);
                    return RedirectToAction("Index");
                }
                return View(proj);
            }
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var proj = entities.Projects.Find(id);
                    // update the information
                    proj.Name = collection["Name"];
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            using (var entities = new ScrumDB())
            {
                var proj = entities.Projects.Find(id);
                if (proj == null)
                {
                    ViewBag.ErrorMessage = string.Format("Project id {0} was not found.", id);
                    return RedirectToAction("Index");
                }
                return View(proj);
            }
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var proj = entities.Projects.Find(id);
                    if (proj == null)
                    {
                        ViewBag.ErrorMessage = string.Format("Project id {0} was not found.", id);
                        return RedirectToAction("Index");
                    }
                    entities.Projects.Remove(proj);
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public ActionResult ProjectBacklog(int id)
        {
            using (var entities = new ScrumDB())
            {
                var proj = entities.Projects.Find(id);
                if (proj == null) return View("Index");
                ViewBag.ProjectID = proj.ID;
                ViewBag.ProjectTitle = proj.Name;
                var projBacklogList = ProjectBacklogItems(proj);
                return View(projBacklogList);
            }
            
        }


        public ActionResult ProjectBoard(int id)
        {
            using (var entities = new ScrumDB())
            {

                var proj = entities.Projects.Find(id);
                if (proj == null) return View("Index");
                ViewBag.ProjectID = proj.ID;
                ViewBag.ProjectTitle = proj.Name;
                var projBacklogList = ProjectBacklogItems(proj);
                return View(projBacklogList);
            }
        }

        /// <summary>
        /// Gets the full list of project backlog items
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        private List<ProjectBacklogItem> ProjectBacklogItems(Project proj)
        {
            var projBacklogList = proj.BacklogItems.Select(backlogItem => new ProjectBacklogItem()
            {
                Name = backlogItem.Name,
                Comments = backlogItem.Comments,
                State = backlogItem.State,
                TaskCount = backlogItem.Tasks.Count,
                Priority = backlogItem.Priority,
                Id = backlogItem.ID
            })
            .Where(o => o.State != ProjectBacklogItem.BacklogItemState.Removed)
            .OrderBy(o => o.Priority)
            .ToList();
            return projBacklogList;
        }

        [HttpGet]
        public ActionResult TeamUserManage(int projectId)
        {
            Project project = null;
            using (var db = new ScrumDB())
            {
                project = db.Projects
                    .Include("Users")
                    .Include("Teams").ToList()
                    .FirstOrDefault(p => p.ID == projectId);

                var projectUsers = db.AspNetUsers.Where(u => u.Projects.Any(p => p.ID == projectId));


                if (project.Teams.Count > 0)
                {
                    var projectTeams = db.Teams.ToList().Except(project.Teams);
                    if (projectTeams.Any()) ViewBag.AllOtherTeams = projectTeams.ToList();
                }
                else
                {
                    ViewBag.AllOtherTeams = db.Teams.ToList();
                }

                if (projectUsers.Any())
                {
                    var projectUserCollection = db.AspNetUsers.ToList().Except(projectUsers);

                    if (projectUserCollection.Any()) ViewBag.AllOtherUsers = projectUserCollection.ToList();
                }
                else
                {
                    ViewBag.AllOtherUsers = db.AspNetUsers.ToList();
                }
            }
            return PartialView("TeamUserManage", project);
        }

        [HttpPost]
        public ActionResult SubmitProjectTeamsUsers(int projectId, int[] teamsArray, string[] usersArray)
        {
            try
            {
                using (var db = new ScrumDB())
                {

                    // get the project
                    var project = db.Projects.Find(projectId);

                    // get all the users
                    var allUsers = db.AspNetUsers.Select(u => u.UserName);
                    var allTeams = db.Teams.Select(t => t.ID);

                    // get the projects's current users
                    var currentProjectUsers = project.Users.Select(u => u.UserName).ToList();
                    var currentProjectTeamIDs = project.Teams.Select(t => t.ID).ToList();

                    // check for no teams
                    if (teamsArray == null || teamsArray.Count() == 0)
                    {
                        foreach (var team in currentProjectTeamIDs)
                        {
                            project.Teams.Remove(db.Teams.FirstOrDefault(t => t.ID == team));
                        }
                    }
                    else
                    {
                        var teamsToAdd = teamsArray.Except(currentProjectTeamIDs).ToArray();
                        var teamsToRemove = allTeams.Except(teamsArray).ToArray();
                        foreach (var teamId in teamsToAdd)
                        {
                            project.Teams.Add(db.Teams.FirstOrDefault(t => t.ID == teamId));
                        }
                        foreach (var teamId in teamsToRemove)
                        {
                            project.Teams.Remove(db.Teams.FirstOrDefault(t => t.ID == teamId));
                        }
                    }

                    // check for no users
                    if (usersArray == null || !usersArray.Any())
                    {
                        foreach (var user in currentProjectUsers)
                        {
                            project.Users.Remove(db.AspNetUsers.FirstOrDefault(u => u.UserName == user));
                        }
                    }
                    else
                    {
                        // get the users to add and remove
                        var usersToAdd = usersArray.Except(currentProjectUsers).ToArray();
                        var usersToRemove = allUsers.Except(usersArray).ToArray();


                        // perform database operations
                        foreach (var userName in usersToRemove)
                        {
                            project.Users.Remove(db.AspNetUsers.FirstOrDefault(r => r.UserName == userName));
                        }
                        foreach (var userName in usersToAdd)
                        {
                            project.Users.Add(db.AspNetUsers.FirstOrDefault(r => r.UserName == userName));
                        }
                    }
                    // save db changes
                    db.SaveChanges();

                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
