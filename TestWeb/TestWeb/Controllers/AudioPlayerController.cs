using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeb.Extensions;

namespace TestWeb.Controllers
{
    public class AudioPlayerController : Controller
    {
        //
        // GET: /AudioPlayer/

        public ActionResult GetFile(String name)
        {
           // string filePath = Server.MapPath(Url.Content(name));

            return new VideoResult(new FileInfo(name)); 
            //File(filePath, "application/octet-stream", filePath);
        }

        public ActionResult PlaySong(Object model)
        {
            return View();
        }

        public ActionResult PlaySongs(Object model)
        {
            return View();
        }

        public ActionResult EmptyAudioPlayer(String name)
        {
            ViewBag.AudioName = name;
            return View();
        }
    }
}
