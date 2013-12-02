using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestWeb.Models.FilesLibrary
{
    public abstract class GeneralFile : AbstractFile
    {
        protected GeneralFile(FileSystemInfo file) : base(file) { }

        public string Extension
        {
            get { return ((FileInfo)File).Extension;  }
        }

        public override DirectoryInfo Parent
        {
            get { return ((FileInfo)File).Directory; }
        }

        public override bool IsFile()
        {
            return true;
        }

        public override bool IsFolder()
        {
            return false;
        }

        public override void Delete()
        {
            ((FileInfo)File).Delete();
        }

        public override void Rename(string newName)
        {
            string extension = ((FileInfo)File).Extension;
            string newPath = ((FileInfo)File).DirectoryName;
            string path = String.Format("{0}\\{1}{2}", newPath, newName, extension);
            System.IO.File.Move(File.FullName, path);
            File = new FileInfo(path);
        }

        public override void Move(string newPath)
        {
            string name = ((FileInfo)File).Name;
            string newLocation = String.Format("{0}\\{1}",newPath,name);
            System.IO.File.Move(((FileInfo)File).FullName, newLocation);
        }

        public override void Copy(string newPath)
        {
            string name = ((FileInfo)File).Name;
            string newLocation = String.Format("{0}\\{1}", newPath, name);
            System.IO.File.Copy(((FileInfo)File).FullName, newLocation);
        }
    }
}
