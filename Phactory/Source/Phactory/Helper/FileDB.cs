using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Project.Helper
{
    static public class FileDB
    {
        static private void GetFilesRecursive(ref List<FileInfo> fileInfos, string path, string extension, bool recursive)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles(extension);

            foreach( FileInfo file in files )
            {
                string nameWithoutExtension = file.Name.Substring(0, file.Name.Length - extension.Length + 1);
                if (nameWithoutExtension.IndexOf(".") == -1)
                {
                    fileInfos.Add(file);
                }
            }

            if (recursive)
            {
                DirectoryInfo[] dirSubs = dir.GetDirectories();
                foreach (DirectoryInfo dirSub in dirSubs)
                {
                    GetFilesRecursive(ref fileInfos, dirSub.FullName, extension, recursive);
                }
            }
        }

        static public List<FileInfo> GetFiles(string path, string extension, bool recursive)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();

            if (Directory.Exists(path))
            {
                GetFilesRecursive(ref fileInfos, path, extension, recursive);
            }

            return fileInfos;
        }

        static private void GetSubDirectoriesRecursive(ref List<DirectoryInfo> dirInfos, string path, bool recursive)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] dirSubs = dir.GetDirectories();
            
            dirInfos.AddRange(dirSubs);

            if (recursive)
            {
                foreach (DirectoryInfo dirSub in dirSubs)
                {
                    GetSubDirectoriesRecursive(ref dirInfos, dirSub.FullName, recursive);
                }
            }
        }

        static public List<DirectoryInfo> GetSubDirectories(string path, bool recursive)
        {
            List<DirectoryInfo> dirInfos = new List<DirectoryInfo>();

            GetSubDirectoriesRecursive(ref dirInfos, path, recursive);

            return dirInfos;
        }
    }
}
