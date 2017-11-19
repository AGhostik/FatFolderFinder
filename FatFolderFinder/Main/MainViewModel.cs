using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;

namespace FatFolderFinder.Main
{
    internal class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _mainModel = new MainModel();

            Size = 0;
            SelectedSizeType = SizeTypeEnum.Mb;
            SizeType.Add(SizeTypeEnum.Byte);
            SizeType.Add(SizeTypeEnum.Kb);
            SizeType.Add(SizeTypeEnum.Mb);
            SizeType.Add(SizeTypeEnum.Gb);

            ExplorerButtonEnabled = false;
            DeleteButtonEnabled = false;

            WaitLabelVisibility = Visibility.Hidden;
        }

        #region Fields

        private readonly MainModel _mainModel;

        private FolderViewModel _selectedItem;
        private bool _explorerButtonEnabled;
        private bool _deleteButtonEnabled;

        private Visibility _waitLabelVisibility;

        #endregion

        #region Properties
        public ObservableCollection<FolderViewModel> Tree { get; set; } = new ObservableCollection<FolderViewModel>();
        public SizeTypeEnum SelectedSizeType { get; set; }
        public ObservableCollection<SizeTypeEnum> SizeType { get; set; } = new ObservableCollection<SizeTypeEnum>();
        public double Size { get; set; }
        public bool ExplorerButtonEnabled { get => _explorerButtonEnabled; set => Set(ref _explorerButtonEnabled, value); }
        public bool DeleteButtonEnabled { get => _deleteButtonEnabled; set => Set(ref _deleteButtonEnabled, value); }

        public FolderViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    ExplorerButtonEnabled = true;
                    DeleteButtonEnabled = true;
                }
                else
                {
                    ExplorerButtonEnabled = false;
                    DeleteButtonEnabled = false;
                }
            }
        }

        public Visibility WaitLabelVisibility
        {
            get => _waitLabelVisibility;
            set => Set(ref _waitLabelVisibility, value);
        }

        #endregion

        #region Metods

        public async Task Scan(string path)
        {
            WaitLabelVisibility = Visibility.Visible;
            Tree.Clear();

            await Task.Run(() =>
            {
                var list = _mainModel.Scan(path, SizeGetAsByte());
                FillTree(list);
            });

            WaitLabelVisibility = Visibility.Hidden;
        }

        public void DeleteFolder()
        {
            _mainModel.DeleteFolder(SelectedItem.FullName);
            RemoveTreeElement(SelectedItem);
        }

        public void OpenFolder()
        {
            _mainModel.OpenFolder(SelectedItem.FullName);
        }

        private void RemoveTreeElement(FolderViewModel element)
        {
            var subTree = Tree;
            FolderViewModel deletedItem = null;
            FolderViewModel deletedItemParent = null;
            var folders = new List<string>(element.FullName.Split(new[] { "\\" }, StringSplitOptions.None));

            for (var i = 0; i < folders.Count; i++)
            {
                foreach (var treeElement in subTree)
                {
                    if ((i == folders.Count - 1) && (treeElement.Name == element.Name))
                    {
                        deletedItem = treeElement;
                        break;
                    }
                    if (treeElement.Name == folders[i])
                    {
                        treeElement.Size -= element.Size;
                        subTree = treeElement.Tree;
                        deletedItemParent = treeElement;
                        break;
                    }
                }
            }

            if (deletedItemParent != null) deletedItemParent.FolderCount--;
            subTree.Remove(deletedItem);            
        }

        private void FillTree(IEnumerable<FolderViewModel> list)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var folderViewModels = list as IList<FolderViewModel> ?? list.ToList();
                UpdateTreeValues(folderViewModels);
                foreach (var item in folderViewModels)
                {
                    Tree.Add(item);
                }
            });            
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
                case SizeTypeEnum.Kb:
                    return value / 1024;
                case SizeTypeEnum.Mb:
                    return value / (1024 * 1024);
                case SizeTypeEnum.Gb:
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
                case SizeTypeEnum.Kb:
                    return (long)(Size * 1024);
                case SizeTypeEnum.Mb:
                    return (long)(Size * (1024 * 1024));
                case SizeTypeEnum.Gb:
                    return (long)(Size * (1024 * 1024 * 1024));
                default:
                    throw new NotImplementedException("SelectedSizeType: unknow value");
            }
        }

        #endregion
    }
}