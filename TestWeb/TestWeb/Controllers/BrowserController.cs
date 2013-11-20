using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class BrowserController : Controller
    {
        //
        // GET: /Browser/

        public ActionResult Browse(Object model)
        {
            return View();
        }

        public ActionResult DeleteFile(Object model)
        {
            return View();
        }

        public ActionResult MoveFile(Object model)
        {
            return View();
        }

        public ActionResult CopyFile(Object model)
        {
            return View();
        }

        public ActionResult RenameFile(Object model)
        {
            return View();
        }

        public ActionResult CreateDirectory(Object model)
        {
            return View();
        }

        public ActionResult ChangeDirectory(Object model)
        {
            return View();
        }

        public ActionResult Upload(Object model)
        {
            return View();
        }
    }
}
