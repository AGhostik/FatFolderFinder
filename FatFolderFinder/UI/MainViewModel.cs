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
            _mainModel.ScanFinished += BuildResultItems;

            ResultItems = new ObservableCollection<object>();

            StartFolder = @"D:\New folder";
            Size = 0;
            SizeType = new ObservableCollection<string>() { "Byte", "KB", "MB", "GB" };
        }
        
        #region Fields

        private IMainModel _mainModel;

        private string _path;
        private long _size;

        #endregion

        #region Properties

        public string StartFolder { get => _path; set => Set(ref _path, value); }
        public long Size { get => _size; set => Set(ref _size, value); } //bytes
        public ObservableCollection<string> SizeType { get; set; }
        public ObservableCollection<object> ResultItems { get; set; }

        #endregion

        #region Metods

        public void Scan()
        {
            _mainModel.Path = StartFolder;
            _mainModel.SizeLimit = Size;
            _mainModel.StartScan();
        }

        private void BuildResultItems(object sender, EventArgs e)
        {
            ResultItems.Clear();

            if (true) //replace to success message
            {
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
            else
            {
            }
        }

        #endregion
    }
}
