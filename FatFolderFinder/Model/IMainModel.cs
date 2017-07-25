using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FatFolderFinder.Model
{
    interface IMainModel
    {
        string Path { get; set; }

        long SizeLimit { get; set; }

        List<object> Folders { get; set; }

        event EventHandler ScanFinished;

        void StartScan();
    }
}
