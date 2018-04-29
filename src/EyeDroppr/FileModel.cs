using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace EyeDroppr
{
    public class FileModel
    {
        public string FileName { get; set; }
        public BitmapImage FileImage { get; set; }
        public Task<BitmapImage> ImageSourceAsync { get; set; }
    }
}