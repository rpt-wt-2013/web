using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Extensions
{
    public class VideoResult : ActionResult
    {
        private FileInfo file;
        public VideoResult(FileInfo file)
        {
            this.file = file;
        }

        public override void ExecuteResult(ControllerContext context)
        {

            context.HttpContext.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", file.Name) );
            context.HttpContext.Response.AddHeader("Content-Range", String.Format("bytes 0-{0}/{0}", file.Length));

            //Check the file exist,  it will be written into the response 
            if (file.Exists)
            {
                FileStream stream = file.OpenRead();
                System.Diagnostics.Debug.WriteLine("about to allocate buffer");
                byte[] bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                System.Diagnostics.Debug.WriteLine("about to write to stream");
                context.HttpContext.Response.BinaryWrite(bytesinfile);
                System.Diagnostics.Debug.WriteLine("finished writing to stream");
            } 
        }
    }
}