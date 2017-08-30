using FatFolderFinder.Model;
using System.Collections.ObjectModel;

namespace FatFolderFinder.UI
{
    public class FolderViewModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public double Size { get; set; }
        public SizeTypeEnum SizeType { get; set; }
        public int FileCount { get; set; }
        public int FolderCount { get; set; }

        public ObservableCollection<FolderViewModel> Tree { get; set; } = new ObservableCollection<FolderViewModel>();
    }
}