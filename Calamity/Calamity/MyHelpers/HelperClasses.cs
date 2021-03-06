﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calamity.Models.FilesLibrary;
using System.Runtime.InteropServices;
using System.IO;

namespace Calamity.MyHelpers
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
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

        public static string getMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename + " not found");

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception)
            {
                return "unknown/unknown";
            }
        }
    }

    public class IDHelper
    {
        private static int id;

        public static void ResetId()
        {
            id = 0;
        }

        public static String GetId()
        {
            return String.Format("{0}",id++);
        }
    }
}