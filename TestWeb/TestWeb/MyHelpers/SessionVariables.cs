using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWeb.Models.FilesLibrary;

namespace TestWeb.MyHelpers
{
    public class SessionVariables
    {
        public static FileLoader GetFileLoader()
        {
            return HttpContext.Current.Session["FileLoader"] as FileLoader;
        }

        public static WorkingFolder GetWorkingFolder()
        {
            return HttpContext.Current.Session["WorkingFolder"] as WorkingFolder;
        }

        public static void setWorkingFolder(WorkingFolder folder)
        {
            HttpContext.Current.Session.Remove("WorkingFolder");
            HttpContext.Current.Session.Add("WorkingFolder", folder);
        }
    }

    public class MimeHelp
    {
        public static String DecideMime(String extension)
        {
            switch (extension)
            {
                case ".txt" :
                    return "text/plain";
                default :
                    return null;
            }
        }
    }
}