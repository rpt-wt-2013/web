using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Calamity.Models.FilesLibrary
{
    public class AudioFile : GeneralFile
    {
        public AudioFile(FileSystemInfo file) : base(file) { }

        public override void DefaultAction()
        {
            Console.WriteLine("This is a lovely audio file");
            Console.WriteLine("Its path is:");
            Console.WriteLine(File.FullName);
        }
    }
}
