using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrumTool.Database;
using ScrumTool.Models;
using ScrumTool.Objects;

namespace ScrumTool.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            using (var entities = new ScrumDB())
            {
                var user = entities.AspNetUsers.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
                if (user != null) ViewBag.UserRoles = user.AspNetRoles.ToList();
                return View();
            }
        }

        // GET: Settings/Admin
        public ActionResult Admin()
        {
            List<AspNetUser> aspNetUserList;
            using (var entities = new ScrumDB())
            {
                aspNetUserList = entities.AspNetUsers.Include("AspNetRoles").ToList();
            }
            return View(aspNetUserList);
        }

        [HttpPost]
        public ActionResult ResetPassword(string username)
        {
            using (var entities = new ScrumDB())
            {
                var user = Membership.GetUser(username);
                if (user != null)
                {
                    var newPassword = user.ResetPassword();
                    return new HttpStatusCodeResult(HttpStatusCode.OK, newPassword);
                }
                else
                {
                    return new HttpNotFoundResult();
                }
            }
        }

        [HttpGet]
        public ActionResult UserGroups(string username)
        {
            UserManage manageModel = new UserManage(username);
            return PartialView(manageModel);
        }

        [HttpPost]
        public ActionResult ChangeUserRoles(string[] roles, string username)
        {
            try
            {
                using (var db = new ScrumDB())
                {
                    // get all the user roles
                    var allUserRoles = db.AspNetRoles.Select(r => r.Name).ToList();

                    // get the user
                    var user = db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

                    // get the user's current roles
                    var currentUserRolesCollection = user.AspNetRoles;
                    var currentUserRoles = currentUserRolesCollection.Select(r => r.Name).ToList();


                    if (roles == null || !roles.Any())
                    {
                        foreach (var role in currentUserRoles)
                        {
                            user.AspNetRoles.Remove(db.AspNetRoles.FirstOrDefault(r => r.Name == role));
                        }
                    }
                    else
                    {
                        // get the roles to add and remove
                        var rolesToAdd = roles.Except(currentUserRoles).ToArray();
                        var rolesToRemove = allUserRoles.Except(roles).ToArray();

                        // perform database operations
                        foreach (var item in rolesToRemove)
                        {
                            user.AspNetRoles.Remove(db.AspNetRoles.FirstOrDefault(r => r.Name == item));
                        }
                        foreach (var item in rolesToAdd)
                        {
                            user.AspNetRoles.Add(db.AspNetRoles.FirstOrDefault(r => r.Name == item));
                        }
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