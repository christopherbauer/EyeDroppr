using System.Windows.Input;
using Windows.UI.Xaml.Media;

namespace EyeDroppr.ViewModels
{
    public class FileViewModel : ViewModelBase
    {
        public string FileName { get; set; }
        public ImageSource FileImage { get; set; }

        public ICommand ClickPhotoCommand { get; set; }
    }
}