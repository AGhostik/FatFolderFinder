using FatFolderFinder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace FatFolderFinder.UI
{
    class MainViewModel
    {
        public MainViewModel()
        {
            _mainModel = new MainModel();

            Size = 0;
            SelectedSizeType = SizeTypeEnum.MB;
            SizeType.Add(SizeTypeEnum.Byte);
            SizeType.Add(SizeTypeEnum.KB);
            SizeType.Add(SizeTypeEnum.MB);
            SizeType.Add(SizeTypeEnum.GB);

            ExplorerButtonEnabled = false;
            DeleteButtonEnabled = false;
        }

        #region Fields

        private readonly MainModel _mainModel;

        #endregion

        #region Properties
        public ObservableCollection<FolderViewModel> Tree { get; set; } = new ObservableCollection<FolderViewModel>();
        public SizeTypeEnum SelectedSizeType { get; set; }
        public ObservableCollection<SizeTypeEnum> SizeType { get; set; } = new ObservableCollection<SizeTypeEnum>();
        public double Size { get; set; }
        public bool ExplorerButtonEnabled { get; set; }
        public bool DeleteButtonEnabled { get; set; }

        #endregion

        #region Metods

        public void Scan(string path)
        {
            Thread thread = new Thread(() => {
                var list = _mainModel.Scan(path, SizeGetAsByte());
                FillTree(list);
            });
            thread.Start();
        }

        public void DeleteFolder()
        {
           // _mainModel.DeleteFolder(Tree[i].FullName);
        }

        public void OpenFolder()
        {
           // _mainModel.OpenFolder(Tree[i].FullName);
        }

        private void FillTree(IEnumerable<FolderViewModel> list)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                Tree.Clear();
                UpdateTreeValues(list);
                foreach (var item in list)
                {
                    Tree.Add(item);
                }
            }));            
        }        

        private void UpdateTreeValues(IEnumerable<FolderViewModel> list)
        {
            foreach (var item in list)
            {
                var editableItem = item;
                editableItem.Size = SizeGetAsCurrent(editableItem.Size);
                editableItem.LocalSize = SizeGetAsCurrent(editableItem.LocalSize);
                editableItem.SizeType = SelectedSizeType;

                if (item.Tree.Count > 0)
                {
                    UpdateTreeValues(item.Tree);
                }                
            }
        }

        private double SizeGetAsCurrent(double value)
        {
            switch (SelectedSizeType)
            {
                case SizeTypeEnum.Byte:
                    return value;
                case SizeTypeEnum.KB:
                    return value / 1024;
                case SizeTypeEnum.MB:
                    return value / (1024 * 1024);
                case SizeTypeEnum.GB:
                    return value / (1024 * 1024 * 1024);
                default:
                    throw new NotImplementedException("SelectedSizeType: unknow value");
            }
        }

        private long SizeGetAsByte()
        {
            switch (SelectedSizeType)
            {
                case SizeTypeEnum.Byte:
                    return (long)Size;
                case SizeTypeEnum.KB:
                    return (long)(Size * 1024);
                case SizeTypeEnum.MB:
                    return (long)(Size * (1024 * 1024));
                case SizeTypeEnum.GB:
                    return (long)(Size * (1024 * 1024 * 1024));
                default:
                    throw new NotImplementedException("SelectedSizeType: unknow value");
            }
        }

        #endregion
    }
}