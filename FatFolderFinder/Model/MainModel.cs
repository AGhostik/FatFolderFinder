using FatFolderFinder.UI;
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
            Folders = new List<object>();
        }
        
        public string Path { get; set; }
        public long SizeLimit { get; set; }
        public List<object> Folders { get; set; }

        public event EventHandler ScanFinished;

        public void StartScan()
        {
            Folders.Clear();

            if (Directory.Exists(Path))
            {
                RecursionScanFolder(new DirectoryInfo(Path));
                ScanFinished(this, null); // replace null to success message arg
            }
            else
            {
                //add event with custom arg
                //fail message arg
                return;
            }
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

                if (size >= SizeLimit)
                {
                    ResultItemViewModel item = new ResultItemViewModel()
                    {
                        Files = files.Count(),
                        Name = d.Name,
                        FullName = d.FullName,
                        Size = size,
                        SizeType = "Byte_!",
                        SubFolders = directories.Count()
                    };
                    Folders.Add(item);
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
