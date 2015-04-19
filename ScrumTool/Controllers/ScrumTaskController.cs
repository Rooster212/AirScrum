using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using ScrumTool.Database;

namespace ScrumTool.Controllers
{
    public class ScrumTaskController : BaseController
    {
        //// GET: ScrumTask
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: ScrumTask/Details/5
        public ActionResult Details(int id)
        {
            using (var entities = new ScrumDB())
            {
                var backlogItem = entities.Tasks.Find(id);
                return View(backlogItem);
            }
        }

        // GET: ScrumTask/Create
        public ActionResult Create(int? id)
        {
            if (id == null) return View();
            ViewBag.BacklogItemID = id;
            return View();
        }

        // POST: ScrumTask/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                int backlogItemId = -1;
                using (var entities = new ScrumDB())
                {
                    backlogItemId = int.Parse(collection["BacklogItemID"]);
                    entities.Tasks.Add(new ScrumTask
                    {
                        Name = collection["Name"],
                        BacklogItemID = backlogItemId,
                        Weight = int.Parse(collection["Weight"]),
                        Comments = collection["Comments"]
                    });
                    entities.SaveChanges();
                }
                if (backlogItemId > -1)
                    return RedirectToAction("Details", "BacklogItem", new {id = backlogItemId});
                else
                    throw new Exception("Backlog item ID is invalid.");
            }
            catch (Exception e)
            {
                return View(); // TODO: change this
            }
        }

        // GET: ScrumTask/Edit/5
        public ActionResult Edit(int id)
        {
            using (var entities = new ScrumDB())
            {
                return View(entities.Tasks.Find(id));
            }
        }

        // POST: ScrumTask/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var item = entities.Tasks.Find(id);
                    if (item == null) return RedirectToAction("Details", new { id = id });

                    item.Name = collection["Name"];
                    item.Weight = int.Parse(collection["Weight"]);
                    item.Comments = collection["Comments"];
                    entities.SaveChanges();

                    return View("Details",item);
                }          
            }
            catch (Exception e)
            {
                return RedirectToAction("Details", new { id = id });
            }
        }

        // POST: ScrumTask/Remove/5
        [HttpPost]
        public ActionResult Remove(int id)
        {
            try
            {
                using (var entities = new ScrumDB())
                {
                    var item = entities.Tasks.Find(id);
                    if(item == null) return new HttpNotFoundResult();
                    entities.Tasks.Remove(item);
                    entities.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            catch (Exception)
            {
                // TODO: something here
                throw;
            }
        }

        //// GET: ScrumTask/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ScrumTask/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
