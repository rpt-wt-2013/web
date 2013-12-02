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

        public ActionResult Browse()
        {
            WorkingFolder wfolder = SessionVariables.GetWorkingFolder();
            if (wfolder.Name == User.Identity.Name)
            {
                return View("Root", wfolder);
            }
            else
            {
                return View("Folder", wfolder);
            }
        }

        public ActionResult Root()
        {
            String user = String.Format("e:/Dropbox/projects/c#/TestWeb/TestWeb/Users/{0}", User.Identity.Name);
            System.Diagnostics.Debug.WriteLine("starting browser in root");
            DirectoryInfo dinfo = new DirectoryInfo(user);
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder(dinfo);
            System.Diagnostics.Debug.WriteLine("created working directory for root");
            SessionVariables.setWorkingFolder(wfolder);
            return RedirectToAction("Browse");
        }

        public ActionResult Folder(int id = 0)
        {
            List<AbstractFile> files = SessionVariables.GetWorkingFolder().GetFiles();
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder((DirectoryInfo)files[id].File);
            SessionVariables.setWorkingFolder(wfolder);
            return RedirectToAction("Browse");
        }

        public ActionResult ParentDirectory()
        {
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder cfolder = SessionVariables.GetWorkingFolder();
            if(cfolder.Name != User.Identity.Name)
            {
                WorkingFolder wfolder = fl.LoadWorkingFolder(cfolder.Parent);
                SessionVariables.setWorkingFolder(wfolder);
            }
            return RedirectToAction("Browse");
        }
        
        public FilePathResult DownloadFile(int id = 0)
        {
            AbstractFile file = SessionVariables.GetWorkingFolder().GetFiles()[id];
            return File(file.FullName, MimeHelp.DecideMime(file.File.Extension), file.Name);
        }

        public ActionResult Upload()
        {
            foreach (string upload in Request.Files)
            {
                if (!Request.Files[upload].HasFile()) continue;
                string path = SessionVariables.GetWorkingFolder().FullName;
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(path, filename));
            }
            return RedirectToAction("Browse");
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
    }
}
