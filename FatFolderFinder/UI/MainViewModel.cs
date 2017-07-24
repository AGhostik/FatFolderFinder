using FatFolderFinder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace FatFolderFinder.UI
{
    class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _mainModel = new MainModel();
            Folders = _mainModel.Folders;
            Size = 1073741824;
        }
        
        #region Fields

        private IMainModel _mainModel;

        private string _path;
        private long _size;
        private string _sizeType;

        #endregion

        #region Properties

        public string Path { get => _path; set => Set(ref _path, value); }
        public long Size { get => _size; set => Set(ref _size, value); } //bytes
        public string SizeType { get => _sizeType; set => Set(ref _sizeType, value); }
        public ObservableCollection<string> Folders { get; set; }

        #endregion

        #region Metods

        public void Scan()
        {
            _mainModel.StartScan(Path, Size);
        }

        #endregion
    }
}
