using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class PlaylistManagerController : Controller
    {
        //
        // GET: /PlaylistManager/

        public ActionResult CreatePlaylist(Object model)
        {
            return View();
        }

        public ActionResult AddSongs(Object model)
        {
            return View();
        }

        public ActionResult RemoveSongs(Object model)
        {
            return View();
        }

        public ActionResult RenamePlaylist(Object model)
        {
            return View();
        }

        public ActionResult DeletePlaylist(Object model)
        {
            return View();
        }
    }
}
