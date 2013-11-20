using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestWeb.Models.FilesLibrary
{
    public class FileLoader
    {
        private HashSet<String> audioExtensions;
        private HashSet<String> videoExtensions;
        private HashSet<String> textExtensions;
        
        public FileLoader() 
        { 
            audioExtensions = new HashSet<string>();
            videoExtensions = new HashSet<string>();
            textExtensions = new HashSet<string>();
        }

        public AbstractFile LoadAbstractFile(FileSystemInfo finfo)
        {
            if(finfo is DirectoryInfo)
            {
                return LoadFolder((DirectoryInfo) finfo);
            }
            else if (finfo is FileInfo)
            {
                return LoadFile((FileInfo)finfo);
            }
            else
            {
                throw new AbsurdException("For unknown reason, FileSystemInfo is not an instance of FileInfo or DirectoryInfo");
            }
        }

        public AbstractFile LoadWorkingFolder(DirectoryInfo dirInfo)
        {
            WorkingFolder folder;
            folder = new WorkingFolder(dirInfo);
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                folder.AddFile(LoadFolder(dir));
            }
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                folder.AddFile(LoadFile(file));
            }
            return folder;
        }

        private AbstractFile LoadFolder(DirectoryInfo dirInfo)
        {
            Folder folder;
            folder = new Folder(dirInfo);
            return folder;
        }

        private AbstractFile LoadFile(FileInfo finfo)
        {
            AbstractFile file = null;
            string extension = finfo.Extension;
            if(textExtensions.Contains(extension)) 
            {
                file = new TextFile(finfo);
            }
            else if (audioExtensions.Contains(extension)) 
            {
                file = new AudioFile(finfo);
            }
            else if (videoExtensions.Contains(extension)) 
            {
                file = new VideoFile(finfo);
            }
            else 
            {
                file = new UnknownFile(finfo);
            }
            return file;
        }

        public bool addAudioExtension(String extension)
        {
            return audioExtensions.Add(extension);
        }

        public bool addVideoExtension(String extension)
        {
            return videoExtensions.Add(extension);
        }

        public bool addTextExtension(String extension)
        {
            return textExtensions.Add(extension);
        }
    }
}
