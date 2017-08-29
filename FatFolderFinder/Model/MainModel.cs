using FatFolderFinder.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace FatFolderFinder.Model
{
    class MainModel : IExplorer
    {
        public MainModel()
        {
        }

        public List<FolderViewModel> Scan(string path, long sizeLimit)
        {
            if (Directory.Exists(path))
            {
                return RecursionScanFolder(new DirectoryInfo(path), sizeLimit);
            }
            else
            {
                throw new ArgumentException(path + " does not exist");
            }
        }

        public void OpenFolder(string path)
        {
            Process.Start(path);
        }

        public void DeleteFolder(string path)
        {
            FileSystem.DeleteDirectory(path,
                UIOption.AllDialogs, 
                RecycleOption.SendToRecycleBin, 
                UICancelOption.DoNothing);
        }

        private List<FolderViewModel> RecursionScanFolder(DirectoryInfo d, long sizeLimit)
        {
            var result = new List<FolderViewModel>();
            var folder = new FolderViewModel();
            
            FileInfo[] files = d.GetFiles();
            foreach (FileInfo f in files)
            {
                folder.Size += f.Length;
            }

            DirectoryInfo[] directories = d.GetDirectories();
            foreach (DirectoryInfo di in directories)
            {
                foreach (var childFolder in RecursionScanFolder(di, sizeLimit))
                {
                    result.Add(childFolder);
                    folder.Size += childFolder.Size;
                }
            }

            if (folder.Size >= sizeLimit)
            {
                folder.Name = d.Name;
                folder.FullName = d.FullName;
                folder.FileCount = d.GetFiles().Length;
                folder.FolderCount = d.GetDirectories().Length;
                folder.SizeType = SizeTypeEnum.Byte;

                result.Add(folder);
            }

            return result;
        }        
    }
}
