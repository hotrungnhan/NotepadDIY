using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NotepadDIY.Components
{
    static class DirAndFileExt
    {
        public static bool RenameFolder(string directory, string newFolderName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(directory) ||
                    string.IsNullOrWhiteSpace(newFolderName))
                {
                    return false;
                }


                var oldDirectory = new DirectoryInfo(directory);

                if (!oldDirectory.Exists)
                {
                    return false;
                }

                if (string.Equals(oldDirectory.Name, newFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    //new folder name is the same with the old one.
                    return false;
                }

                string newDirectory;

                if (oldDirectory.Parent == null)
                {
                    //root directory
                    newDirectory = Path.Combine(directory, newFolderName);
                }
                else
                {
                    newDirectory = Path.Combine(oldDirectory.Parent.FullName, newFolderName);
                }

                if (Directory.Exists(newDirectory))
                {
                    //target directory already exists
                    return false;
                }

                oldDirectory.MoveTo(newDirectory);

                return true;
            }
            catch
            {
                //ignored
                return false;
            }
        }
        public static void RenameFile(string oldfile, string newName)
        {
            var info = new FileInfo(oldfile);
            info.MoveTo(Path.Combine(info.Directory.FullName, newName));

        }
        public static bool isDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                return true;
            }
            return false;
        }
    }
}