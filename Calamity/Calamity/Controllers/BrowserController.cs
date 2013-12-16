using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calamity.Models;
using Calamity.Models.FilesLibrary;
using Calamity.MyHelpers;
using DatabaseApplication.Database;
using System.Data;

namespace Calamity.Controllers
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
        public ActionResult UploadFinished()
        {
            List<AbstractFile> oldFiles = new List<AbstractFile>(SessionVariables.GetWorkingFolder().GetFiles());

            SessionVariables.GetFileLoader().RefreshWorkingFolder(SessionVariables.GetWorkingFolder());

            List<AbstractFile> newFiles = SessionVariables.GetWorkingFolder().GetFiles();

            SqliteDatabase db = new SqliteDatabase(User.Identity.Name);

            String workingDir = Calamity.MyHelpers.SessionVariables.GetWorkingFolder().FullName;
            // C:\\Users\\Patrik\\Desktop\\WT\\Calamity\\Calamity\\Users\\

            string pattern = ".*.Users.";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);

            List<AbstractFile> result = newFiles.Except(oldFiles).ToList();
            foreach (AbstractFile file in result)
            {
                if (file.IsFile())
                {
                    if (file is AudioFile)
                    {
                        string path = rgx.Replace(file.FullName, "");

                        db.ExecuteNonQuery("insert into audio_files(path, name) values ('" + path.Replace("'", "''") + "', '" + file.Name + "')");
                    }
                    else if (file is VideoFile)
                    {
                        string path = rgx.Replace(file.FullName, "");

                        db.ExecuteNonQuery("insert into video_files(path, name) values ('" + path.Replace("'", "''") + "', '" + file.Name + "')");
                    }
                }
            }

            db.Dispose();

            return RedirectToAction("Browse", "Browser");
        }

        [Authorize]
        public ActionResult Root()
        {
            String user = String.Format("C:\\Users\\Patrik\\Desktop\\WT\\Calamity\\Calamity\\Users\\{0}", User.Identity.Name);
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
            if (cfolder.Name != User.Identity.Name)
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
            WorkingFolder wf = SessionVariables.GetWorkingFolder();
            foreach (string upload in Request.Files)
            {
                if (!Request.Files[upload].HasFile()) continue;
                System.Diagnostics.Debug.Write(System.DateTime.Now);
                System.Diagnostics.Debug.WriteLine("starting upload");
                string path = wf.FullName;
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(path, filename));
                System.Diagnostics.Debug.Write(System.DateTime.Now);
                System.Diagnostics.Debug.WriteLine("finished uploading");
            }
            SessionVariables.GetFileLoader().RefreshWorkingFolder(wf);
            return RedirectToAction("Browse");
        }

        public PartialViewResult FileTree(String folder, String view)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            return PartialView(view, dir.GetDirectories());
        }

        public ActionResult DeleteFile(String name)
        {
            FileLoader fl = SessionVariables.GetFileLoader();
            AbstractFile file = fl.LoadAbstractFile(name.GetFSInfo());

            SqliteDatabase db = new SqliteDatabase(User.Identity.Name);
            
            string pattern = ".*.Users.";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);

            if (file.IsFile())
            {
                if (file is AudioFile)
                {
                    string path = rgx.Replace(file.FullName, "");

                    db.ExecuteNonQuery("delete from playlists where audio_id = (select audio_id from audio_files where path like '" + path + "%')");
                    db.ExecuteNonQuery("delete from audio_files where path like '" + path + "'");
                }
                else if (file is VideoFile)
                {
                    string path = rgx.Replace(file.FullName, "");

                    db.ExecuteNonQuery("delete from video_files where path like '" + path + "'");
                }
            }
            else if (file.IsFolder())
            {
                string path = rgx.Replace(file.FullName, "");

                db.ExecuteNonQuery("delete from playlists where audio_id = (select audio_id from audio_files where path like '" + path + "%')");

                db.ExecuteNonQuery("delete from audio_files where path like '" + path + "%'");
                db.ExecuteNonQuery("delete from video_files where path like '" + path + "%'");
            }

            db.Dispose();

            file.Delete();
            fl.RefreshWorkingFolder(SessionVariables.GetWorkingFolder());
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
                FileLoader fl = SessionVariables.GetFileLoader();
                AbstractFile file = fl.LoadAbstractFile(model.File.GetFSInfo());
                System.Diagnostics.Debug.WriteLine("move file, model is:");
                System.Diagnostics.Debug.WriteLine(file.FullName);
                System.Diagnostics.Debug.WriteLine(model.NewPath);

                SqliteDatabase db = new SqliteDatabase(User.Identity.Name);

                string pattern = ".*.Users.";
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);

                if (file.IsFile())
                {
                    if (file is AudioFile)
                    {
                        string newPath = rgx.Replace(model.NewPath, "");
                        string oldPath = rgx.Replace(file.FullName, "");

                        db.ExecuteNonQuery("update audio_files set path = '" + String.Format("{0}\\{1}", newPath, file.Name).Replace("'", "''") + "' where path like '" + oldPath + "'");
                    }
                    else if (file is VideoFile)
                    {
                        string newPath = rgx.Replace(model.NewPath, "");
                        string oldPath = rgx.Replace(file.FullName, "");

                        db.ExecuteNonQuery("update video_files set path = '" + String.Format("{0}\\{1}", newPath, file.Name).Replace("'", "''") + "' where path like '" + oldPath + "'");
                    }
                }
                else if (file.IsFolder())
                {
                    string oldPath = rgx.Replace(file.FullName, "");
                    string newPath = rgx.Replace(model.NewPath, "");

                    string patt = User.Identity.Name + ".";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(patt);

                    DataTable audio_files = db.GetDataTable("select * from audio_files where path like '" + oldPath + "%'");
                    foreach (DataRow row in audio_files.Rows)
                    {
                        string path = (string) row["path"];
                        string reducedPath = regex.Replace(path, "");

                        db.ExecuteNonQuery("update audio_files set path = '" + String.Format("{0}\\{1}", newPath, reducedPath).Replace("'", "''") + "' where path like '" + path + "'");
                    }

                    DataTable video_files = db.GetDataTable("select * from video_files where path like '" + oldPath + "%'");
                    foreach (DataRow row in video_files.Rows)
                    {
                        string path = (string)row["path"];
                        string reducedPath = regex.Replace(path, "");

                        db.ExecuteNonQuery("update video_files set path = '" + String.Format("{0}\\{1}", newPath, reducedPath).Replace("'", "''") + "' where path like '" + path + "'");
                    }
                }

                db.Dispose();

                file.Move(model.NewPath);
                fl.RefreshWorkingFolder(SessionVariables.GetWorkingFolder());
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
                SqliteDatabase db = new SqliteDatabase(User.Identity.Name);

                string pattern = ".*.Users.";
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);

                if (file.IsFile())
                {
                    if (file is AudioFile)
                    {
                        string path = rgx.Replace(model.NewPath, "");

                        db.ExecuteNonQuery("insert into audio_files(path, name) values ('" + String.Format("{0}\\{1}", path, file.Name).Replace("'", "''") + "', '" + file.Name + "')");
                    }
                    else if (file is VideoFile)
                    {
                        string path = rgx.Replace(model.NewPath, "");

                        db.ExecuteNonQuery("insert into video_files(path, name) values ('" + String.Format("{0}\\{1}", path, file.Name).Replace("'", "''") + "', '" + file.Name + "')");
                    }
                }
                else if (file.IsFolder())
                {
                    string oldPath = rgx.Replace(file.FullName, "");
                    string newPath = rgx.Replace(model.NewPath, "");

                    string patt = User.Identity.Name + ".";
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(patt);

                    DataTable audio_files = db.GetDataTable("select * from audio_files where path like '" + oldPath + "%'");
                    foreach (DataRow row in audio_files.Rows)
                    {
                        string path = (string)row["path"];
                        string reducedPath = regex.Replace(path, "");

                        string name = (string)row["name"];

                        db.ExecuteNonQuery("insert into audio_files(path, name) values ('" + String.Format("{0}\\{1}", newPath, reducedPath).Replace("'", "''") + "', '" + name + "')");
                    }

                    DataTable video_files = db.GetDataTable("select * from video_files where path like '" + oldPath + "%'");
                    foreach (DataRow row in video_files.Rows)
                    {
                        string path = (string)row["path"];
                        string reducedPath = regex.Replace(path, "");

                        string name = (string)row["name"];

                        db.ExecuteNonQuery("insert into video_files(path, name) values ('" + String.Format("{0}\\{1}", newPath, reducedPath).Replace("'", "''") + "', '" + name + "')");
                    }
                }

                db.Dispose();

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
            if (ModelState.IsValid)
            {
                FileLoader fl = SessionVariables.GetFileLoader();
                AbstractFile file = fl.LoadAbstractFile(model.File.GetFSInfo());
                System.Diagnostics.Debug.WriteLine("rename file, model is:");
                System.Diagnostics.Debug.WriteLine(file.FullName);
                System.Diagnostics.Debug.WriteLine(model.NewName);

                SqliteDatabase db = new SqliteDatabase(User.Identity.Name);

                string pattern = ".*.Users.";
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);

                string oldPath = rgx.Replace(file.FullName, "");

                file.Rename(model.NewName);

                if (file is AudioFile)
                {
                    string newPath = rgx.Replace(file.FullName, "");

                    db.ExecuteNonQuery("update audio_files set path = '" + newPath.Replace("'", "''") + "', name = '" + file.Name + "' where path like '" + oldPath + "'");
                }
                else if (file is VideoFile)
                {
                    string newPath = rgx.Replace(file.FullName, "");

                    db.ExecuteNonQuery("update video_files set path = '" + newPath.Replace("'", "''") + "', name = '" + file.Name + "' where path like '" + oldPath + "'");
                }

                db.Dispose();

                fl.RefreshWorkingFolder(SessionVariables.GetWorkingFolder());
            }
            return RedirectToAction("Browse");
        }

        public ActionResult CreateDirectory()
        {
            return View(new CreateFolderModel());
        }

        [HttpPost]
        public ActionResult CreateDirectory(CreateFolderModel model)
        {
            System.Diagnostics.Debug.WriteLine("starting create");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("create folder, model is:");
                System.Diagnostics.Debug.WriteLine(model.FolderName);
                WorkingFolder wf = SessionVariables.GetWorkingFolder();
                String path = String.Format("{0}/{1}", wf.FullName, model.FolderName);
                System.Diagnostics.Debug.WriteLine("create folder, path is:");
                System.Diagnostics.Debug.WriteLine(path);
                Directory.CreateDirectory(path);
                SessionVariables.GetFileLoader().RefreshWorkingFolder(wf);
            }
            return RedirectToAction("Browse");
        }
    }
}
