using FatFolderFinder.Model;
using System;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace FatFolderFinder.UI
{
    class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _mainModel = new MainModel();
            _mainModel.UpdateFolders += BuildResultItems;

            ResultItems = new ObservableCollection<object>();
            
            SizeType = new ObservableCollection<string>()
            {
                b,
                kb,
                mb,
                gb
            };
            SelectedSizeType = b;
        }

        #region Fields

        private const string b = "Byte";
        private const string kb = "KB";
        private const string mb = "MB";
        private const string gb = "GB";

        private IMainModel _mainModel;

        private string _path;
        private string _selectedSizeType;
        private double _size;

        #endregion

        #region Properties

        public string StartFolder { get => _path; set => Set(ref _path, value); }
        public double Size { get => _size; set => Set(ref _size, value); }
        public string SelectedSizeType { get => _selectedSizeType; set => Set(ref _selectedSizeType, value); }
        public ObservableCollection<string> SizeType { get; set; }
        public ObservableCollection<object> ResultItems { get; set; }

        #endregion

        #region Metods

        public void Scan()
        {
            _mainModel.Path = StartFolder;
            _mainModel.SizeLimit = Size;
            _mainModel.SizeType = SelectedSizeType;
            _mainModel.StartScan();
        }

        public void OpenFolder()
        {
            _mainModel.OpenCheckedFolders();
        }

        public void DeleteFolder()
        {
            _mainModel.DeleteCheckedFolders();
        }

        private void BuildResultItems(object sender, EventArgs e)
        {
            ResultItems.Clear();

            var folders = _mainModel.Folders;
            foreach (var folder in folders)
            {
                ResultItem item = new ResultItem()
                {
                    DataContext = folder
                };
                ResultItems.Add(item);
            }
        }

        #endregion
    }
}
