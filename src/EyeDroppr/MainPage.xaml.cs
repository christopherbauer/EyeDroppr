using System.IO;

using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EyeDroppr
{
    using System;
    using System.Threading.Tasks;

    using Windows.Storage;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var absolutePath = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "Assets\\Wood-sm.jpg");

            var currentFile = Task.Run(() => StorageFile.GetFileFromPathAsync(absolutePath).AsTask()).Result;

            DataContext = new MainPageViewModel { CurrentFile = currentFile };
        }
    }
}
