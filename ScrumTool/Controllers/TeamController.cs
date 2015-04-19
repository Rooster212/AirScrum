using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScrumTool.Database;
using WebGrease.Css.Extensions;

namespace ScrumTool.Controllers
{
    [Authorize]
    public class TeamController : BaseController
    {
        // GET: Team
        public ActionResult Index()
        {
            using (var entities = new ScrumDB())
            {
                return View(entities.Teams.ToList());
            }
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {
            using (var entities = new ScrumDB())
            {
                try
                {
                    var team = entities.Teams.Find(id);
                    var teamUsers = entities.AspNetUsers.Where(u => u.Teams.Any(t => t.ID == id))
                        .Include("AspNetRoles").ToList();
                    ViewBag.teamUsers = teamUsers;
                    return View(team);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    entities.Teams.Add(new Team()
                    {
                        Name = collection["Name"]
                    });
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            using (var entities = new ScrumDB())
            {
                var team = entities.Teams.Find(id);
                return View(team);
            }
        }

        // POST: Team/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var team = entities.Teams.Find(id);
                    if (team == null) throw new InstanceNotFoundException();
                    team.Name = collection["Name"];
                    entities.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (InstanceNotFoundException exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: Team/Delete/5
        public ActionResult Delete(int id)
        {
            using (var entities = new ScrumDB())
            {
                var team = entities.Teams.Find(id);
                if (team == null)
                {
                    ViewBag.ErrorMessage = string.Format("Team with ID {0} not found.", id);
                    return RedirectToAction("Index");
                }
                return View(team);
            }
        }

        // POST: Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var team = entities.Teams.Find(id);
                    if (team == null)
                    {
                        ViewBag.ErrorMessage = string.Format("Team with ID {0} not found.", id);
                        return RedirectToAction("Index");
                    }
                    entities.Teams.Remove(team);
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult AddUserToTeam(int teamId, string username)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var team = entities.Teams.Find(teamId);
                    team.Users.Add(entities.AspNetUsers.FirstOrDefault(u => u.UserName == username));
                    entities.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public ActionResult Manage(int team)
        {
            try
            {
                using (var db = new ScrumDB())
                {
                    var allUsersInTeam = db.AspNetUsers.Where(u => u.Teams.Any(t => t.ID == team));

                    ViewBag.AllOtherUsers = db.AspNetUsers.Except(allUsersInTeam).ToList();

                    return PartialView("Manage", 
                        db.Teams.Where(t => t.ID == team)
                            .Include("Users")
                            .Include("Users.AspNetRoles")
                            .ToList()
                            .FirstOrDefault());
                }
            }
            catch (Exception e)
            {         
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public ActionResult TeamUsers(string[] users, int teamId )
        {
            try
            {
                using (var db = new ScrumDB())
                {
                    // get all the users
                    var allUsers = db.AspNetUsers.Select(u => u.UserName);

                    // get the team
                    var team = db.Teams.Find(teamId);

                    // get the team's current users
                    var currentTeamUsers = team.Users;
                    var currentTeamUsersList = currentTeamUsers.Select(u => u.UserName).ToList();

                    
                    // if there are no users in the list we just need to remove them all
                    if (users == null || users.Length == 0)
                    {
                        foreach (var user in currentTeamUsersList)
                        {
                            team.Users.Remove(db.AspNetUsers.FirstOrDefault(u => u.UserName == user));
                        }
                        db.SaveChanges();
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }

                    // get the users to add and remove
                    var usersToAdd = users.Except(currentTeamUsersList).ToArray();
                    var usersToRemove = allUsers.Except(users).ToArray();

                    // perform database operations
                    foreach (var item in usersToRemove)
                    {
                        team.Users.Remove(db.AspNetUsers.FirstOrDefault(r => r.UserName == item));
                    }
                    foreach (var item in usersToAdd)
                    {
                        team.Users.Add(db.AspNetUsers.FirstOrDefault(r => r.UserName == item));
                    }

                    // save changes
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
