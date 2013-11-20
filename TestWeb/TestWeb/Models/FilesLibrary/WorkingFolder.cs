using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWeb.Models.FilesLibrary
{
    public class WorkingFolder : Folder
    {
        private List<AbstractFile> files;

        public WorkingFolder(DirectoryInfo folder) : base(folder)
        {
            files = new List<AbstractFile>();
        }

        public void AddFile(AbstractFile file, int index)
        {
            files[index] = file;
        }

        public void AddFile(AbstractFile file)
        {
            files.Add(file);
        }

        public List<AbstractFile> GetFiles()
        {
            return files;
        }

        public AbstractFile GetFile(int index)
        {
            return files[index];
        }
    }
}
