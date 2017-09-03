﻿using FatFolderFinder.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace FatFolderFinder.Model
{
    public class MainModel
    {
        public List<FolderViewModel> Scan(string path, long sizeLimit)
        {
            if (Directory.Exists(path))
            {
                return BuildFolderTree(new DirectoryInfo(path), sizeLimit);
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

        private List<FolderViewModel> BuildFolderTree(DirectoryInfo d, long sizeLimit)
        {
            var result = new List<FolderViewModel>();
            var folder = new FolderViewModel()
            {
                Name = d.Name,
                FullName = d.FullName,
                FileCount = d.GetFiles().Length,
                FolderCount = d.GetDirectories().Length,
                Size = 0,
                LocalSize = 0,
                SizeType = SizeTypeEnum.Byte
            };
            
            FileInfo[] files = d.GetFiles();
            foreach (FileInfo f in files)
            {
                folder.Size += f.Length;
                folder.LocalSize += f.Length;
            }

            DirectoryInfo[] directories = d.GetDirectories();
            foreach (DirectoryInfo di in directories)
            {
                foreach (var childFolder in BuildFolderTree(di, sizeLimit))
                {
                    if (childFolder.Size >= sizeLimit)
                    {
                        folder.Tree.Add(childFolder);
                    }
                    folder.Size += childFolder.Size;
                }
            }

            result.Add(folder);

            return result;
        }        
    }
}
