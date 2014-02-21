using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Project.Helper
{
    static public class StringHelper
    {
        static public string MakeFullPath(string relativePath)
        {
            return App.Controller.UserConfig.ResourcePath + relativePath;
        }

        static public string MakeRelativePath(string fullPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(App.Controller.UserConfig.ResourcePath);
            FileInfo fileInfo = new FileInfo(fullPath);

            return fileInfo.FullName.Substring(dirInfo.FullName.Length, fileInfo.FullName.Length - dirInfo.FullName.Length);
        }

        static public FileInfo GetFileInfo(PhactoryHost.Database.Resource resource)
        {
            return resource.FileInfo;
        }

        static public string GetFileExtension(string filename)
        {
            int dotIndex = filename.LastIndexOf(".");

            if (dotIndex == -1)
            {
                return ".";
            }

            return filename.Substring(dotIndex, filename.Length - dotIndex).ToLower();
        }
    }
}
