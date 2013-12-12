using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeb.Models;
using TestWeb.Models.FilesLibrary;
using TestWeb.MyHelpers;

namespace TestWeb.Controllers
{
    public class BrowserController : Controller
    {
        // GET: /Browser/

        [Authorize]
        public ActionResult Browse()
        {
            WorkingFolder wfolder = SessionVariables.GetWorkingFolder();
            if (wfolder == null)
            {
                return RedirectToAction("Root", "Browser");
            }
            if (wfolder.Name == User.Identity.Name)
            {
                return View("Root", wfolder);
            }
            else
            {
                return View("Folder", wfolder);
            }
        }

        [Authorize]
        public ActionResult Root()
        {
            String user = String.Format("E:/Dropbox/Projects/c#/TestWeb/TestWeb/users/{0}", User.Identity.Name);
            System.Diagnostics.Debug.WriteLine("starting browser in root");
            DirectoryInfo dinfo = new DirectoryInfo(user);
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder(dinfo);
            System.Diagnostics.Debug.WriteLine("created working directory for root");
            SessionVariables.setWorkingFolder(wfolder);
            return RedirectToAction("Browse");
        }

        [Authorize]
        public ActionResult Folder(String name)
        {
            //List<AbstractFile> files = SessionVariables.GetWorkingFolder().GetFiles();
            FileLoader fl = SessionVariables.GetFileLoader();
            WorkingFolder wfolder = fl.LoadWorkingFolder(new DirectoryInfo(name));
            SessionVariables.setWorkingFolder(wfolder);
            return RedirectToAction("Browse");
        }

        [Authorize]
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

        [Authorize]
        public FilePathResult DownloadFile(String name)
        {
            return File(name, MimeHelp.getMimeFromFile(name), name);
        }

        [Authorize]
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

        public PartialViewResult FileTree(String folder, String view)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            return PartialView(view, dir.GetDirectories());
        }

        public ActionResult DeleteFile(String name)
        {
            AbstractFile file = SessionVariables.GetFileLoader().LoadAbstractFile(name.GetFSInfo());
            file.Delete();
            return RedirectToAction("Browse");
        }

        public ActionResult MoveFile()
        {
            return View(new MoveModel());
        }

        [HttpPost]
        public ActionResult MoveFile(MoveModel model)
        {
            if (ModelState.IsValid)
            {
                AbstractFile file = SessionVariables.GetFileLoader().LoadAbstractFile(model.File.GetFSInfo());
                System.Diagnostics.Debug.WriteLine("move file, model is:");
                System.Diagnostics.Debug.WriteLine(file.FullName);
                System.Diagnostics.Debug.WriteLine(model.NewPath);
                file.Move(model.NewPath);
            }
            return RedirectToAction("Browse");
        }

        public ActionResult CopyFile()
        {
            return View(new CopyModel());
        }

        [HttpPost]
        public ActionResult CopyFile(CopyModel model)
        {
            if (ModelState.IsValid)
            {
                AbstractFile file = SessionVariables.GetFileLoader().LoadAbstractFile(model.File.GetFSInfo());
                System.Diagnostics.Debug.WriteLine("copy file, model is:");
                System.Diagnostics.Debug.WriteLine(file.FullName);
                System.Diagnostics.Debug.WriteLine(model.NewPath);
                file.Copy(model.NewPath);
            }
            return RedirectToAction("Browse");
        }

        public ActionResult RenameFile()
        {
            return View(new RenameModel());
        }
        
        [HttpPost]
        public ActionResult RenameFile(RenameModel model)
        {
            if(ModelState.IsValid)
            {
                AbstractFile file = SessionVariables.GetFileLoader().LoadAbstractFile(model.File.GetFSInfo());
                System.Diagnostics.Debug.WriteLine("rename file, model is:");
                System.Diagnostics.Debug.WriteLine(file.FullName);
                System.Diagnostics.Debug.WriteLine(model.NewName);
                file.Rename(model.NewName);
            }
            return RedirectToAction("Browse");
        }

        
        public ActionResult CreateDirectory(String name)
        {
            WorkingFolder wf = SessionVariables.GetWorkingFolder();
            Directory.CreateDirectory(wf.FullName+name);
            return RedirectToAction("Browse");
        }
    }
}
