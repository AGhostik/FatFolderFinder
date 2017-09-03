using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FatFolderFinder.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;            
        }

        MainViewModel _mainViewModel;
        
        private void FolderDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                _mainViewModel.Scan(dialog.FileName);
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
    }
}
