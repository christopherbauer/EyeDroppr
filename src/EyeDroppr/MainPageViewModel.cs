using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace EyeDroppr
{
    using Windows.Storage;
    using Windows.Storage.Pickers;

    using Core;

    public static class Logger
    {
        private static readonly Stopwatch AppTimer;
        private static LoggerMode _mode = LoggerMode.Total;
        private static TimeSpan _lastLog;

        static Logger()
        {
            AppTimer = new Stopwatch();
            AppTimer.Start();
        }

        public static void SetMode(LoggerMode mode)
        {
            _mode = mode;
        }

        public static void Log(string messge, [CallerMemberName]string callerMemberName = null)
        {
            var logTime = AppTimer.Elapsed;
            TimeSpan reportLogTime;
            if (_mode == LoggerMode.Difference)
            {
                reportLogTime = logTime.Subtract(_lastLog);
            }
           else if (_mode == LoggerMode.Total)
           {
               reportLogTime = logTime;
           }
            _lastLog = logTime;
            Debug.WriteLine("{3}{0}: {1} ({2})", reportLogTime, messge, callerMemberName, (_mode == LoggerMode.Difference ? "+" : string.Empty));
        }
    }

    public enum LoggerMode
    {
        Difference,
        Total
    }

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
                     FileImage = CurrentImage
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

            Make = metaData[SystemProperty.CameraManufacturer];
            Model = metaData[SystemProperty.CameraModel];
            ExposureTime = string.Format("1/{0}",1/Convert.ToDouble(metaData[SystemProperty.ExposureTime]));
            ISOSpeed = metaData[SystemProperty.ISOSpeed];
            FStop = string.Format("f/{0}", metaData[SystemProperty.FStop]);
            ExposureBias = metaData[SystemProperty.ExposureBias];

            var filterer = new ColorFilterer();
            Logger.Log("Color Counts await start");
            var filteredColors = filterer.GetTopColors(await colorCounts, 10);
            Logger.Log("Color Counts await end");

            TopColors = filteredColors.Select(pair => new ColorStatistics { Color = pair.Key, Count = pair.Value }).ToList();
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
    }
}