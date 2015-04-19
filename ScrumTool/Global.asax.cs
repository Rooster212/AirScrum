using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ScrumTool.Database;
using ScrumTool.Objects;

namespace ScrumTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            //SeedDatabase();
        }

        private void SeedDatabase()
        {
            const int numberOfProjectsToAdd = 2;
            const int numberOfTasksToAdd = 10;
            using (var db = new ScrumDB())
            {
                if (db.Projects.Count() < 2) // if there are no projects
                {
                    for (int i = 1; i <= numberOfProjectsToAdd; i++)
                    {
                        var projectName = "Example Proj " + i;
                        db.Projects.Add(new Project()
                        {
                            Name = projectName
                        });
                        db.SaveChanges();
                        var project = db.Projects.FirstOrDefault(p => p.Name == projectName);
                        if (project == null) continue;
                        Random rnd = new Random();

                        var projectId = project.ID;
                        for (int BLI = 0; BLI < numberOfTasksToAdd; BLI++)
                        {
                            var backlogItemName = "Backlog Item " + BLI;
                            db.BacklogItems.Add(new BacklogItem()
                            {
                                ProjectID = projectId,
                                Name = backlogItemName,
                                State = (ProjectBacklogItem.BacklogItemState)rnd.Next(0, 2),
                                Comments = "Hello world this is a comment",
                                Priority = numberOfTasksToAdd-BLI // put the tasks in backwards just for the lols
                            });
                            db.SaveChanges();

                            var backlogItem = db.BacklogItems.FirstOrDefault(bli => bli.Name == backlogItemName);
                            for (int j = 0; j < numberOfTasksToAdd; j++)
                            {
                                db.Tasks.Add(new ScrumTask()
                                {
                                    BacklogItem = backlogItem,
                                    Name = "New Task "+j,
                                    Weight = 8,
                                    Priority = j
                                });

                                db.SaveChanges();
                            }
                        }
                    }   
                }

                if (!db.Teams.Any()) // if there are no teams
                {
                    for (int i = 0; i < 100; i++)
                    {

                    }
                }

                
            }
        }
    }
}
