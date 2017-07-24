using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FatFolderFinder.Model
{
    interface IMainModel
    {
        ObservableCollection<string> Folders { get; set; }

        void StartScan(string path, long size);
    }
}
