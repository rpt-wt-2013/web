using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Calamity.Models.FilesLibrary
{
    public abstract class AbstractFile
    {
        protected FileSystemInfo file;

        public FileSystemInfo File
        {
            get { return file; }
            set { file = value; }
        }

        protected AbstractFile(FileSystemInfo file)
        {
            this.file = file;
        }

        public abstract void DefaultAction();

        public abstract bool IsFile();

        public abstract bool IsFolder();

        public abstract void Delete();

        public abstract void Rename(string newName);

        public abstract void Move(string newPath);

        public abstract void Copy(string newPath);

        public DateTime LastAccessTime
        {
            get { return file.LastAccessTime; }
            set { file.LastAccessTime = value; }
        }

        public DateTime LastWriteTime
        {
            get { return file.LastWriteTime; }
            set { file.LastWriteTime = value; }
        }

        public DateTime CreationTime
        {
            get { return file.CreationTime; }
            set { file.CreationTime = value; }
        }

        public string Name
        {
            get { return file.Name; }
        }

        public string FullName
        {
            get { return file.FullName; }
        }

        public abstract DirectoryInfo Parent { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AbstractFile) )
            {
                return false;
            }
            else
            {
                return file.FullName.Equals(((AbstractFile)obj).File.FullName);
            }
        }

        public override int GetHashCode()
        {
            return file.FullName.GetHashCode();
        }
    }
}
