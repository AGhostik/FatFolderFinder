using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatFolderFinder.Model
{
    class MainModel : IMainModel
    {
        public MainModel()
        {
            Folders = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Folders { get; set; }

        public void StartScan(string path, long size)
        {
            if (Directory.Exists(path))
            {
                DirectorySize(new DirectoryInfo(path), size);
            }
            else
            {
                return;
            }
        }

        private long DirectorySize(DirectoryInfo d, long sizeLimit)
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
                    size += DirectorySize(di, sizeLimit);
                }

                if (size >= sizeLimit)
                {
                    Folders.Add(d.FullName);
                }

                return size;
            }
            catch
            {
                return 0;
            }
        }
    }
}
