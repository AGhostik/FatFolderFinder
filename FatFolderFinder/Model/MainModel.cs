using FatFolderFinder.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Windows;

namespace FatFolderFinder.Model
{
    class MainModel : IMainModel
    {
        public MainModel()
        {
            Folders = new List<object>();
        }

        private const string b = "Byte";
        private const string kb = "KB";
        private const string mb = "MB";
        private const string gb = "GB";

        private long _sizeLimitByte;

        public string Path { get; set; }
        public double SizeLimit { get; set; }
        public List<object> Folders { get; set; }
        public string SizeType { get; set; }

        public event EventHandler UpdateFolders;

        public void StartScan()
        {
            Folders.Clear();

            if (Directory.Exists(Path))
            {
                _sizeLimitByte = GetSizeInBytes(SizeLimit);
                RecursionScanFolder(new DirectoryInfo(Path));
                UpdateFolders(this, null);
            }
        }

        public void OpenCheckedFolders()
        {
            foreach (var folder in Folders)
            {
                ResultItemViewModel f = folder as ResultItemViewModel;
                if (f.IsChecked)
                {
                    Process.Start(f.FullName);
                }
            }
        }

        public void DeleteCheckedFolders()
        {
            for (int i = 0; i < Folders.Count; i++)
            {
                ResultItemViewModel f = Folders[i] as ResultItemViewModel;

                if (f.IsChecked)
                {
                    FileSystem.DeleteDirectory(f.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);

                    i -= RemoveFolderTree(f.FullName);
                }
            }
            UpdateFolders(this, null);
        }

        private int RemoveFolderTree(string rootFolder)
        {
            int removedCount = 0;
            for (int i = 0; i < Folders.Count; i++)
            {
                ResultItemViewModel f = Folders[i] as ResultItemViewModel;
                if (f.FullName.Contains(rootFolder))
                {
                    Folders.RemoveAt(i);                    
                    removedCount++;
                    i--;
                }
            }
            return removedCount;
        }

        private long RecursionScanFolder(DirectoryInfo d)
        {
            long size = 0;

            try //can catch access denied error, if has no administrative privileges
            {
                FileInfo[] files = d.GetFiles();
                foreach (FileInfo f in files)
                {
                    size += f.Length;
                }                     

                DirectoryInfo[] directories = d.GetDirectories();
                foreach (DirectoryInfo di in directories)
                {
                    size += RecursionScanFolder(di);
                }

                if (size >= _sizeLimitByte)
                {
                    AddFolder(d, size, files.Count(), directories.Count());
                }

                return size;
            }
            catch
            {
                return 0;
            }
        }

        private void AddFolder(DirectoryInfo dir, long size, int fileCount, int subdirCount)
        {
            ResultItemViewModel item = new ResultItemViewModel()
            {
                Files = fileCount,
                Name = dir.Name,
                FullName = dir.FullName,
                Size = Math.Round(GetSizeInSelectedType(size), 2),
                SizeType = SizeType,
                SubFolders = subdirCount
            };
            Folders.Add(item);
        }


        private long GetSizeInBytes(double size)
        {
            switch (SizeType)
            {
                case b:
                    return (long) size;
                case kb:
                    return (long) size * 1024;
                case mb:
                    return (long) size * 1024 * 1024;
                case gb:
                    return (long) size * 1024 * 1024 * 1024;
                default:
                    return 0;
            }
        }

        private double GetSizeInSelectedType(long size)
        {
            switch (SizeType)
            {
                case b:
                    return size;
                case kb:
                    return (double) size / 1024;
                case mb:
                    return (double) size / (1024 * 1024);
                case gb:
                    return (double) size / (1024 * 1024 * 1024);
                default:
                    return 0;
            }
        }

    }
}
