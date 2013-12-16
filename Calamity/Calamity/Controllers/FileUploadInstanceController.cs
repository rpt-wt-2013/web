using Backload;
using Calamity.Models.FilesLibrary;
using Calamity.MyHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Calamity.Controllers
{

    public class FileUploadInstanceController : Controller
    {
        public async Task<ActionResult> FileHandler()
        {
            Debug.WriteLine("HAF HAF HAF");
            FileUploadHandler handler = new FileUploadHandler(Request, this);
            handler.StoreFileRequestFinished += handler_StoreFileRequestFinished;
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;

            ActionResult result = await handler.HandleRequestAsync();  
            return result;
        }

        void handler_IncomingRequestStarted(object sender, Backload.Eventing.Args.IncomingRequestEventArgs e)
        {
            Debug.WriteLine("HAF HAF HAF");
        }

        void handler_StoreFileRequestFinished(object sender, Backload.Eventing.Args.StoreFileRequestEventArgs e)
        {
            SessionVariables.GetFileLoader().RefreshWorkingFolder(SessionVariables.GetWorkingFolder());
            RedirectToAction("Browse");
        }

    }
}
