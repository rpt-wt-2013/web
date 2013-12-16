using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Calamity.MyHelpers
{
    public static class ExtensionMethods
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }

        public static FileSystemInfo GetFSInfo(this String path)
        {
            System.IO.FileAttributes fa = System.IO.File.GetAttributes(path);
            if ((fa & FileAttributes.Directory) != 0)
            {
                return new DirectoryInfo(path);
            }
            else
            {
                return new FileInfo(path);
            }
        }
    }
}