using FatFolderFinder.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FatFolderFinder.UI
{
    class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _mainModel = new MainModel();

            Size = 0;
            SelectedSizeType = SizeTypeEnum.Byte;
            SizeType.Add(SizeTypeEnum.Byte);
            SizeType.Add(SizeTypeEnum.KB);
            SizeType.Add(SizeTypeEnum.MB);
            SizeType.Add(SizeTypeEnum.GB);

            ExplorerButtonEnabled = false;
            DeleteButtonEnabled = false;
        }

        #region Fields

        private MainModel _mainModel;

        private SizeTypeEnum _selectedSizeType;
        private int _selectedItem;

        #endregion

        #region Properties
        public ObservableCollection<FolderViewModel> Tree { get; set; } = new ObservableCollection<FolderViewModel>();
        public SizeTypeEnum SelectedSizeType
        {
            get => _selectedSizeType;
            set => _selectedSizeType = value;
        }
        public ObservableCollection<SizeTypeEnum> SizeType { get; set; } = new ObservableCollection<SizeTypeEnum>();
        public double Size { get; set; }
        public int SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }
        public bool ExplorerButtonEnabled { get; set; }
        public bool DeleteButtonEnabled { get; set; }

        #endregion

        #region Metods

        public void Scan(string path)
        {
            var list = _mainModel.Scan(path, SizeGetAsByte());
            FillTree(list);
        }

        public void DeleteFolder()
        {
            _mainModel.DeleteFolder(Tree[SelectedItem].FullName);
        }

        public void OpenFolder()
        {
            _mainModel.OpenFolder(Tree[SelectedItem].FullName);
        }

        private void FillTree(IEnumerable<FolderViewModel> list)
        {
            Tree.Clear();
            ResetTreeValues(list);
            foreach (var item in list)
            {
                Tree.Add(item);
            }
        }

        private void ResetTreeValues(IEnumerable<FolderViewModel> list)
        {
            foreach (var item in list)
            {
                var editableItem = item;
                editableItem.Size = SizeGetAsCurrent(editableItem.Size);
                editableItem.SizeType = SelectedSizeType;

                if (item.Tree.Count > 0)
                {
                    ResetTreeValues(item.Tree);
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