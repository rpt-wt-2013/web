using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class AudioPlayerController : Controller
    {
        //
        // GET: /AudioPlayer/

        public ActionResult PlaySong(Object model)
        {
            return View();
        }

        public ActionResult PlaySongs(Object model)
        {
            return View();
        }

        public ActionResult EmptyAudioPlayer(Object model)
        {
            return View();
        }
    }
}
