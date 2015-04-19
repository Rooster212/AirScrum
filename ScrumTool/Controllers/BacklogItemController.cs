using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScrumTool.Database;
using ScrumTool.Objects;

namespace ScrumTool.Controllers
{
    public class BacklogItemController : BaseController
    {
        // GET: BacklogItem
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: BacklogItem/Details/5
        public ActionResult Details(int id)
        {
            using (var entities = new ScrumDB())
            {

                var item = entities.BacklogItems.Find(id);
                ViewBag.BacklogItemTaskList = item.Tasks.ToList();
                return View(item);
            }
        }

        // GET: BacklogItem/Create
        public ActionResult Create(int? id)
        {
            if (id == null) return View();
            var item = new BacklogItem
            {
                ProjectID = (int) id
            };
            return View(item);
        }

        // POST: BacklogItem/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    entities.BacklogItems.Add(new BacklogItem()
                    {
                        Name = collection["Name"],
                        ProjectID = int.Parse(collection["ProjectID"]),
                        Comments = collection["Comments"],
                        State = 0
                    });
                    entities.SaveChanges();
                }
                return RedirectToAction("ProjectBacklog","Project",new{id = collection["ProjectID"]});
            }
            catch
            {
                return View();
            }
        }

        // GET: BacklogItem/Edit/5
        public ActionResult Edit(int id)
        {
            using (var entities = new ScrumDB())
            {
                return View(entities.BacklogItems.Find(id));
            }
        }

        // POST: BacklogItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                try
                {
                    using (var entities = new ScrumDB())
                    {
                        var bli = entities.BacklogItems.Find(id);
                        bli.Name = collection["Name"];
                        bli.ProjectID = int.Parse(collection["ProjectID"]);
                        bli.Comments = collection["Comments"];
                        bli.State = (ProjectBacklogItem.BacklogItemState)int.Parse(collection["State"]);

                        entities.SaveChanges();
                    }
                    return RedirectToAction("ProjectBacklog", "Project", new { id = collection["ProjectID"] });
                }
                catch
                {
                    return View();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BacklogItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BacklogItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult StateChange(int backlogItemId, int newState)
        {
            using (var entities = new ScrumDB())
            {
                try
                {
                    var item = entities.BacklogItems.Find(backlogItemId);
                    item.State = (ProjectBacklogItem.BacklogItemState)newState;
                    item.Priority = null;
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult PriorityChange(int projId, int[] idArray)
        {
            using (var entities = new ScrumDB())
            {
                try
                {
                    var itemList = entities.BacklogItems.Where(i => i.ProjectID == projId && i.State != ProjectBacklogItem.BacklogItemState.Removed);

                    foreach (var item in itemList)
                    {
                        item.Priority = Array.IndexOf(idArray, item.ID); // assign the priority of the item according to it's position in the array
                    }

                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult BacklogItemTasks(int id)
        {
            using (var entities = new ScrumDB())
            {
                var backlogItem = entities.BacklogItems.Find(id);
                if (backlogItem == null) return HttpNotFound();
                var tasks = backlogItem.Tasks.ToList();
                return PartialView("_TaskListPartial", tasks);
            }
        }

    }
}
