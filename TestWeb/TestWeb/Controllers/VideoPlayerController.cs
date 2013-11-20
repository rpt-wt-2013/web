using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class VideoPlayerController : Controller
    {
        //
        // GET: /VideoPlayer/

        public ActionResult PlayVideo(Object model)
        {
            return View();
        }

        public ActionResult EmptyVideoPlayer(Object model)
        {
            return View();
        }
    }
}
