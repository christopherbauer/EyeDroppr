using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace EyeDroppr
{
    using Windows.Storage;
    using Windows.Storage.Pickers;

    using Core;

    /// <summary>
    /// The main page view model.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private IList<ColorStatistics> _topColors;
        private IStorageFile _currentFile;
        private string _make;
        private string _model;
        private string _exposureTime;
        private string _isoSpeed;
        private string _fStop;
        private IList<FileModel> _folderContents;
        private ImageSource _currentImage;
        private string _exposureBias;
        protected string _watchFolderLocation;
        private string _aperture;

        public MainPageViewModel()
        {
            GetFileCommand = new ActionCommand(this.GetFile);
            ChangeFolderCommand = new ActionCommand(this.ChangeFolder);
        }

        private async void ChangeFolder()
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            folderPicker.ViewMode = PickerViewMode.List;
            folderPicker.CommitButtonText = "Watch";
            var folder = await folderPicker.PickSingleFolderAsync();
            WatchFolderLocation = folder.Path;

            var filesAsync = await folder.GetFilesAsync();
            var filesList = new List<FileModel>();
            foreach (var storageFile in filesAsync)
            {
                filesList.Add(new FileModel
                {
                     FileName = storageFile.Name,
                     FileImage = await GetImageSource(storageFile)
                });
            }
            FolderContents = filesList;
        }

        private async void GetFile()
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".gif");
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            filePicker.CommitButtonText = "Open";
            CurrentFile = await filePicker.PickSingleFileAsync();
            CurrentImage = await GetImageSource(CurrentFile);

            var statistics = new ImageStatistics();

            Logger.Log("Metadata start");
            var data = statistics.GetMetaData(CurrentFile);

            Logger.Log("Color Counts start");
            var colorCounts = statistics.GetColorCounts(CurrentFile);
            
            Logger.Log("Metadata await start");
            var metaData = await data;
            Logger.Log("Metadata await end");

            Make = DisplayProperty<string>(metaData, SystemProperty.CameraManufacturer);
            Model = DisplayProperty<string>(metaData, SystemProperty.CameraModel);
            ExposureTime = $"1/{1 / DisplayProperty<double>(metaData, SystemProperty.ExposureTime)}";
            ISOSpeed = DisplayProperty<string>(metaData, SystemProperty.ISOSpeed);
            FStop = $"f/{DisplayProperty<string>(metaData, SystemProperty.FStop)}";
            ExposureBias = DisplayProperty<string>(metaData, SystemProperty.ExposureBias);
            Aperture = DisplayProperty<string>(metaData, SystemProperty.Aperture);

            var filterer = new ColorFilterer();
            Logger.Log("Color Counts await start");
            var filteredColors = filterer.GetTopColors(await colorCounts, 10);
            Logger.Log("Color Counts await end");

            TopColors = filteredColors.Select(pair => new ColorStatistics { Color = pair.Key, Count = pair.Value }).ToList();
        }

        private static T DisplayProperty<T>(IDictionary<SystemProperty, string> metaData, SystemProperty systemProperty)
        {
            if (metaData.ContainsKey(systemProperty))
            {
                var s = metaData[systemProperty];
                return (T) Convert.ChangeType(s, typeof(T));
            }
            return default(T);
        }

        public string ExposureBias
        {
            get { return _exposureBias; }
            set { SetValue(ref _exposureBias, value); }
        }

        public ImageSource CurrentImage
        {
            get { return _currentImage; }
            set { SetValue(ref _currentImage, value); }
        }

        private async Task<BitmapImage> GetImageSource(IStorageFile storageFile)
        {
            var bitmapImage = new BitmapImage();

            using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                bitmapImage.SetSource(stream);
            }
            return bitmapImage;
        }

        public IStorageFile CurrentFile
        {
            get { return _currentFile; }
            set { SetValue(ref _currentFile, value); }
        }

        public IList<ColorStatistics> TopColors
        {
            get { return this._topColors; }
            set { this.SetValue(ref this._topColors, value); }
        }

        /// <summary>
        /// Gets or sets the get file command.
        /// </summary>
        public ICommand GetFileCommand { get; set; }
        public ICommand ChangeFolderCommand { get; set; }

        public string Make
        {
            get { return _make; }
            set { SetValue(ref _make, value); }
        }

        public string Model
        {
            get { return _model; }
            set { SetValue(ref _model, value);}
        }

        public string ExposureTime
        {
            get { return _exposureTime; }
            set { SetValue(ref _exposureTime, value); }
        }

        public string ISOSpeed
        {
            get { return _isoSpeed; }
            set { SetValue(ref _isoSpeed, value); }
        }

        public string FStop
        {
            get { return _fStop; }
            set { SetValue(ref _fStop, value); }
        }

        public IList<FileModel> FolderContents
        {
            get { return _folderContents; }
            set { SetValue(ref _folderContents, value); }
        }

        public string WatchFolderLocation
        {
            get { return _watchFolderLocation; }
            set { SetValue(ref _watchFolderLocation, value);}
        }

        public string Aperture
        {
            get { return _aperture; }
            set
            {
                SetValue(ref _aperture, value);
                OnPropertyChanged(nameof(HasAperture));
            }
        }

        public string HasAperture => string.IsNullOrEmpty(Aperture) ? "Collapsed" : "Visible";
    }
}