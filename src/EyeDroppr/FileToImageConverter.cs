using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace EyeDroppr
{
    public class FileToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var storageFile = value as StorageFile;
            return GetImageAsync(storageFile).Result;
        }

        private static async Task<ImageSource> GetImageAsync(IStorageFile storageFile)
        {
            var bitmapImage = new BitmapImage();
            
            var stream = await storageFile.OpenAsync(FileAccessMode.Read).AsTask().ConfigureAwait(false);
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}