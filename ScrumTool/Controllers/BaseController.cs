using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace ScrumTool.Controllers
{
    public class BaseController : Controller
    {
        protected BaseController()
        {
            bool inDebug = false;
#if DEBUG
            inDebug = true;
#endif
            ViewBag.InDebug = inDebug;
        }
    }
}