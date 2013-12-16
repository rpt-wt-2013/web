using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamity.Models.FilesLibrary
{
    public class TextFile : GeneralFile
    {
        public TextFile(FileSystemInfo file) : base(file) { }

        public override void DefaultAction()
        {
            Console.WriteLine("This is a lovely text file");
            Console.WriteLine("Its path is:");
            Console.WriteLine(File.FullName);
        }
    }
}
