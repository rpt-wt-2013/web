using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWeb.Models.FilesLibrary
{
    public class UnknownFile : GeneralFile
    {
        public UnknownFile(FileSystemInfo file) : base(file) { }

        public override void DefaultAction()
        {
            Console.WriteLine("This is a lovely unknown file");
            Console.WriteLine("Its path is:");
            Console.WriteLine(File.FullName);
        }
    }
}
