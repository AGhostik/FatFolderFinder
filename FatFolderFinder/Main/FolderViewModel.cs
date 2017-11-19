using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace FatFolderFinder.Main
{
    public class FolderViewModel : ObservableObject
    {
        private string _name;
        private string _fullName;
        private double _size;
        private double _localSize;
        private SizeTypeEnum _sizeType;
        private int _fileCount;
        private int _folderCount;

        public ObservableCollection<FolderViewModel> Tree { get; set; } = new ObservableCollection<FolderViewModel>();

        public string Name { get => _name; set => Set(ref _name, value); }
        public string FullName { get => _fullName; set => Set(ref _fullName, value); }
        public double Size { get => _size; set => Set(ref _size, value); }
        public double LocalSize { get => _localSize; set => Set(ref _localSize, value); }
        public SizeTypeEnum SizeType { get => _sizeType; set => Set(ref _sizeType, value); }
        public int FileCount { get => _fileCount; set => Set(ref _fileCount, value); }
        public int FolderCount { get => _folderCount; set => Set(ref _folderCount, value); }
    }
}