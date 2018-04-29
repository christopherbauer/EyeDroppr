using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Core;

namespace EyeDroppr.ViewModels
{
    /// <summary>
    /// The main page view model.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private IList<ColorStatisticsViewModel> _topColors;
        private IStorageFile _currentFile;
        
        private IList<FileViewModel> _folderContents;
        private ImageSource _currentImage;
        protected string _watchFolderLocation;
        private PhotoMetaDataViewModel _photoMetaDataViewModel;

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
            var filesList = new List<FileViewModel>();
            foreach (var storageFile in filesAsync)
            {
                filesList.Add(new FileViewModel
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

            PhotoMetaDataViewModel = new PhotoMetaDataViewModel(metaData);

            var filterer = new ColorFilterer();
            Logger.Log("Color Counts await start");
            var filteredColors = filterer.GetTopColors(await colorCounts, 10);
            Logger.Log("Color Counts await end");

            TopColors = filteredColors.Select(pair => new ColorStatisticsViewModel(pair.Key, pair.Value)).ToList();
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

        public IList<ColorStatisticsViewModel> TopColors
        {
            get { return this._topColors; }
            set { this.SetValue(ref this._topColors, value); }
        }

        /// <summary>
        /// Gets or sets the get file command.
        /// </summary>
        public ICommand GetFileCommand { get; set; }
        public ICommand ChangeFolderCommand { get; set; }


        public IList<FileViewModel> FolderContents
        {
            get { return _folderContents; }
            set { SetValue(ref _folderContents, value); }
        }

        public string WatchFolderLocation
        {
            get { return _watchFolderLocation; }
            set { SetValue(ref _watchFolderLocation, value);}
        }

        public PhotoMetaDataViewModel PhotoMetaDataViewModel
        {
            get { return _photoMetaDataViewModel; }
            set { SetValue(ref _photoMetaDataViewModel, value); }
        }
    }
}