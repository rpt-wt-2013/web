using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestWeb.Models.FilesLibrary
{
    public class Folder : AbstractFile
    {
        public Folder(FileSystemInfo file) : base(file) { }

        public override void DefaultAction()
        {
            Console.WriteLine("This is a lovely folder");
            Console.WriteLine("Its path is:");
            Console.WriteLine(File.FullName);
        }

        public override bool IsFile()
        {
            return false;
        }

        public override bool IsFolder()
        {
            return true;
        }

        public override void Delete()
        {
            ((DirectoryInfo) File).Delete(true);
        }

        public override void Rename(string newName)
        {
            String path = ((DirectoryInfo)File).Parent.FullName;
            String newPath = String.Format("{0}\\{1}", path, newName);
            if (!File.FullName.Equals(newPath))
            {
                Directory.Move(File.FullName, newPath);
                File = new DirectoryInfo(newPath);
            }
        }

        public override void Move(string newPath)
        {
            string name = ((DirectoryInfo)File).Name;
            string newLocation = String.Format("{0}\\{1}", newPath,name);
            Directory.Move(((DirectoryInfo)File).FullName, newLocation);
        }

        public override void Copy(string newPath)
        {
            string name = ((DirectoryInfo)File).Name;
            string newLocation = String.Format("{0}\\{1}", newPath, name);
            System.IO.File.Copy(((DirectoryInfo)File).FullName, newLocation);
        }

        public override string Parent
        {
            get
            {
                return ((DirectoryInfo)File).Parent.FullName;
            }
        }
    }
}
