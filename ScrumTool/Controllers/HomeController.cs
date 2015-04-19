using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ScrumTool.Database;
using ScrumTool.Logic;

namespace ScrumTool.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            using (var entities = new ScrumDB())
            {
                if (User.IsInRole("Admin"))
                {
                    ViewBag.PageTitle = "All Projects";

                    // if they are an admin show all the projects
                    var projectSummaryList =
                        entities.Projects.Include("BacklogItems")
                            .Include("BacklogItems.Tasks")
                            .Include("Sprints")
                            .ToList();
                    return View(projectSummaryList);
                }
                else
                {
                    ViewBag.PageTitle = "Your Projects";
                    // check for projects that the user has access to
                    var allowedProjects = ScrumToolAuthorize.AllowedProjects(HttpContext.User.Identity.Name);
                    return View(allowedProjects);
                }
            }
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "SCRUM Tool - About";
            
            return View();
        }

        public ActionResult Changelog()
        {
            return View();
        }
    }
}