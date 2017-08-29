using FatFolderFinder.UI;
using System.Collections.Generic;

namespace FatFolderFinder.Model
{
    interface IExplorer
    {
        List<FolderViewModel> Scan(string path, long sizeLimit);
        void OpenFolder(string path);
        void DeleteFolder(string path);
    }
}
