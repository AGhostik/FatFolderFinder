using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FatFolderFinder
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;
        }

        private readonly MainViewModel _mainViewModel;
        
        private async void FolderDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                await _mainViewModel.Scan(dialog.FileName);
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.OpenFolder();
        }

        private void DeleteFolderButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.DeleteFolder();
        }

        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // наиболее простой костыль из всех возможных костылей
            _mainViewModel.SelectedItem = FolderTree.SelectedItem as FolderViewModel;
        }
    }
}
