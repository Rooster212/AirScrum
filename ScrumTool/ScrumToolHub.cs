using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ScrumTool
{
    public class ScrumToolHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}