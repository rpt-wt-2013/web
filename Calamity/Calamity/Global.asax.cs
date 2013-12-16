using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Calamity.Models.FilesLibrary;

namespace Calamity
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        
        public void Session_Start(object sender, EventArgs e)
        {
            FileLoader fl = new FileLoader();
            fl.addTextExtension(".txt");
            fl.addTextExtension(".doc");
            fl.addTextExtension(".docx");
            fl.addTextExtension(".pdf");
            fl.addAudioExtension(".mp3");
            fl.addVideoExtension(".avi");
            fl.addVideoExtension(".mp4");
            fl.addVideoExtension(".mkv");
            fl.addPictureExtension(".jpg");
            fl.addPictureExtension(".png");
            fl.addPictureExtension(".bmp");
            HttpContext.Current.Session.Add("FileLoader", fl);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // NOTE: If you downloaded the Backload component using NuGet, the RegisterBundles() method is now in ~/App_Start/BackloadConfig.cs
            // Use bundeling for client files (scripts (js) and styles (css)). 
            // Comment this out, if you manually include the files in your page
            // Overload: RegisterBundles(BundleTable.Bundles, pathToScripts, pathToStyles) 
            // Example: Backload.Configuration.FileUploadBundles.RegisterBundles(BundleTable.Bundles, "~/Scripts/FileUpload/", "~/Content/FileUpload/css/");
            // Backload.Configuration.FileUploadBundles.RegisterBundles(BundleTable.Bundles);
        }
    }
}