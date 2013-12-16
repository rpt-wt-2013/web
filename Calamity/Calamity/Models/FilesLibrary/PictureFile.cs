using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Calamity.Models.FilesLibrary
{
    public class PictureFile : GeneralFile
    {
        public PictureFile(FileSystemInfo file) : base(file) { }

        public override void DefaultAction()
        {
            Console.WriteLine("This is a lovely picture file");
            Console.WriteLine("Its path is:");
            Console.WriteLine(File.FullName);
        }
    }
}