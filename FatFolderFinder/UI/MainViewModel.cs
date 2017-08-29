using FatFolderFinder.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace FatFolderFinder.UI
{
    class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _mainModel = new MainModel();

            Size = 0;
            SelectedSizeType = SizeTypeEnum.Byte;            
        }

        #region Fields

        private MainModel _mainModel;

        private SizeTypeEnum _selectedSizeType;
        private long _size;
        private int _selectedItem;

        #endregion

        #region Properties
        
        public ObservableCollection<FolderViewModel> Tree = new ObservableCollection<FolderViewModel>(); // ?

        public SizeTypeEnum SelectedSizeType
        {
            get => _selectedSizeType;
            set => _selectedSizeType = value;
        }

        public ObservableCollection<SizeTypeEnum> SizeType { get; set; }
        public float Size
        {
            get
            {
                switch (SelectedSizeType)
                {
                    case SizeTypeEnum.Byte:
                        return _size;
                    case SizeTypeEnum.KB:
                        return (float)_size / 1024;
                    case SizeTypeEnum.MB:
                        return (float)_size / (1024 * 1024);
                    case SizeTypeEnum.GB:
                        return (float)_size / (1024 * 1024 * 1024);
                    default:
                        return _size;
                }
            }
            set
            {
                switch (SelectedSizeType)
                {
                    case SizeTypeEnum.Byte:
                        _size = (long)value;
                        break;
                    case SizeTypeEnum.KB:
                        _size = (long)value * 1024;
                        break;
                    case SizeTypeEnum.MB:
                        _size = (long)value * 1024 * 1024;
                        break;
                    case SizeTypeEnum.GB:
                        _size = (long)value * 1024 * 1024 * 1024;
                        break;
                }
            }
        }
        public int SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        #endregion

        #region Metods

        public void Scan(string path)
        {
            var list = _mainModel.Scan(path, _size);
            Tree = new ObservableCollection<FolderViewModel>(list);
        }

        public void DeleteFolder()
        {
            _mainModel.DeleteFolder(Tree[SelectedItem].FullName);
        }

        public void OpenFolder()
        {
            _mainModel.OpenFolder(Tree[SelectedItem].FullName);
        }

        #endregion
    }
}