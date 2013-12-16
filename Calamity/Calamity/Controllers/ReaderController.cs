using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calamity.Controllers
{
    public class ReaderController : Controller
    {
        //
        // GET: /Reader/

        public ActionResult ReadDocument(Object model)
        {
            return View();
        }

        public ActionResult EmptyReader(Object model)
        {
            return View();
        }
    }
}
