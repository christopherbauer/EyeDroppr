using Windows.Storage;

namespace EyeDroppr
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Windows.UI;
    using Windows.UI.Xaml;

    /// <summary>
    /// The design main page view model.
    /// </summary>
    public class DesignMainPageViewModel : MainPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignMainPageViewModel"/> class.
        /// </summary>
        public DesignMainPageViewModel()
        {
            this.TopColors = new List<ColorStatistics>
                                 {
                                     new ColorStatistics { Color = Colors.Red, Count = 10 },
                                     new ColorStatistics { Color = Colors.Green, Count = 8 },
                                     new ColorStatistics { Color = Colors.Blue, Count = 6 }
                                 };
            this.Make = "Canon";
            this.Model = "Canon Powershot G12";
            this.ExposureTime = "1/1250 sec";
            this.ISOSpeed = "ISO-80";
            this.FStop = "f/4";
            WatchFolderLocation = "C:/My Pictures";
            FolderContents = new List<FileModel> { new FileModel { FileName = "IMG_0001.png"}, new FileModel { FileName = "IMG_0002.png" },new FileModel { FileName = "IMG_0002(2).png" }, new FileModel { FileName = "IMG_0003.png" }, new FileModel { FileName = "IMG_0004.png" } };
        }
    }
}