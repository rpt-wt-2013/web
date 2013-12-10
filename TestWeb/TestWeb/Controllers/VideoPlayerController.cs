using Lib.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeb.Extensions;

namespace TestWeb.Controllers
{
    public class VideoPlayerController : Controller
    {
        //
        // GET: /VideoPlayer/

        public ActionResult PlayVideo(String name)
        {
            ViewBag.VideoName = name;
            return View();
        }

        public ActionResult EmptyVideoPlayer()
        {
            return View();
        }

        public ActionResult Video(String type, String name)
        {
            FileInfo oceansClipInfo = null;
            string oceansClipMimeType = String.Format("video/{0}", type);

            switch (type)
            {
                case "mp4":
                    //oceansClipInfo = new FileInfo(Server.MapPath(String.Format("~/Content/video/{0}.mp4",name)));
                    oceansClipInfo = new FileInfo(name);
                    break;
                case "webm":
                    oceansClipInfo = new FileInfo(Server.MapPath(String.Format("~/Content/video/{0}.webm", name)));
                    break;
                case "ogg":
                    oceansClipInfo = new FileInfo(Server.MapPath(String.Format("~/Content/video/{0}.ogg", name)));
                    break;
            }

            return new RangeFilePathResult(oceansClipMimeType, oceansClipInfo.FullName, oceansClipInfo.LastWriteTimeUtc, oceansClipInfo.Length);
            
        }
    }
}
