using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeb.Models.FilesLibrary;
using TestWeb.MyHelpers;

namespace TestWeb.Controllers
{
    public class BrowserController : Controller
    {
        // GET: /Browser/

        public ActionResult Root()
        {
            String user = String.Format("e:/Dropbox/projects/c#/TestWeb/TestWeb/Users/{0}", User.Identity.Name);
            System.Diagnostics.Debug.WriteLine("starting browser in root");
            DirectoryInfo dinfo = new DirectoryInfo(user);
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder(dinfo);
            System.Diagnostics.Debug.WriteLine("created working directory for root");
            SessionVariables.setWorkingFolder(wfolder);
            return View("Folder", wfolder);
        }

        public ActionResult Folder(int id = 0)
        {
            List<AbstractFile> files = SessionVariables.GetWorkingFolder().GetFiles();
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder((DirectoryInfo)files[id].File);
            SessionVariables.setWorkingFolder(wfolder);
            return View("Folder", wfolder);
        }

        public ActionResult ParentDirectory()
        {
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder(new DirectoryInfo(SessionVariables.GetWorkingFolder().Parent));
            SessionVariables.setWorkingFolder(wfolder);
            return View("Folder",wfolder);
        }

        public ActionResult DeleteFile()
        {
            return View();
        }

        public ActionResult MoveFile()
        {
            return View();
        }

        public ActionResult CopyFile()
        {
            return View();
        }

        public ActionResult RenameFile()
        {
            return View();
        }

        public ActionResult CreateDirectory()
        {
            return View();
        }

        public ActionResult ChangeDirectory()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }
    }
}
