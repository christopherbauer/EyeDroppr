using System;
using System.Collections.Generic;
using Windows.UI;
using Core;

namespace EyeDroppr.ViewModels.Designer
{
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
            PhotoMetaDataViewModel = new DesignPhotoMetaDataViewModel();
            TopColors = new List<ColorStatisticsViewModel>
                                 {
                                     new ColorStatisticsViewModel { Color = Colors.Red, Count = 10 },
                                     new ColorStatisticsViewModel { Color = Colors.Green, Count = 8 },
                                     new ColorStatisticsViewModel { Color = Colors.Blue, Count = 6 }
                                 };
            WatchFolderLocation = "C:/My Pictures";
            FolderContents = new List<FileViewModel> { new FileViewModel { FileName = "IMG_0001.png"}, new FileViewModel { FileName = "IMG_0002.png" },new FileViewModel { FileName = "IMG_0002(2).png" }, new FileViewModel { FileName = "IMG_0003.png" }, new FileViewModel { FileName = "IMG_0004.png" } };
        }
    }
}