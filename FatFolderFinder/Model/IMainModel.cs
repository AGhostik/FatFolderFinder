using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FatFolderFinder.Model
{
    interface IMainModel
    {
        string Path { get; set; }

        string SizeType { get; set; }

        double SizeLimit { get; set; }

        List<object> Folders { get; set; }

        event EventHandler UpdateFolders;

        void StartScan();

        void OpenCheckedFolders();

        void DeleteCheckedFolders();
    }
}
