using FatFolderFinder.Model;

namespace FatFolderFinder.UI
{
    class ResultItemViewModel
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public double Size { get; set; }
        public SizeTypeEnum SizeType { get; set; }
        public int Files { get; set; }
        public int SubFolders { get; set; }
    }
}
