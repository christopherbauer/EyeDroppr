// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageStatistics.cs" company="">
//   
// </copyright>
// <summary>
//   The image statistics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Windows.Graphics.Imaging;
    using Windows.Storage;
    using Windows.UI;

    /// <summary>
    /// The image statistics.
    /// </summary>
    public class ImageStatistics
    {
        /// <summary>
        /// The get color counts.
        /// </summary>
        /// <param name="storageFile"></param>
        /// <param name="bitmap">
        /// The bitmap.
        /// </param>
        /// <exception cref="Exception">
        /// The operation failed.
        /// </exception>
        /// <returns>
        /// The <see cref="Dictionary{TKey,TValue}"/>.
        /// </returns>
        public Task<Dictionary<Color, int>> GetColorCounts(IStorageFile storageFile)
        {
            return GetColorCountsAsync(storageFile, 40);
        }

        private static async Task<Dictionary<Color, int>> GetColorCountsAsync(IStorageFile storageFile, int fuzziness)
        {
            var colorCounts = new Dictionary<Color, int>();

            using (var randomAccessStream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
                var pixelData =
                    await decoder.GetPixelDataAsync(
                        decoder.BitmapPixelFormat,
                        decoder.BitmapAlphaMode,
                        new BitmapTransform(), 
                        ExifOrientationMode.IgnoreExifOrientation,
                        ColorManagementMode.ColorManageToSRgb);

                var pixels = pixelData.DetachPixelData();
                Debug.Assert(pixels.Length % 4 == 0);
                for (var i = 0; i < pixels.Length/4; i=i+4)
                {
                    var alpha = pixels[i+3];
                    var red = pixels[i+2];
                    var green = pixels[i+1];
                    var blue = pixels[i];
                    var color = Color.FromArgb(alpha, red, green, blue);

                    foreach (var key in colorCounts.Keys)
                    {
                        if (color.IsInRange(key, fuzziness))
                        {
                            color = key;
                        }
                    }

                    if (!colorCounts.ContainsKey(color))
                    {
                        colorCounts.Add(color, 0);
                    }
                    colorCounts[color] = colorCounts[color] + 1;
                }
            }

            return colorCounts;
        }

        public async Task<IDictionary<SystemProperty, string>> GetMetaData(IStorageFile storageFile)
        {
            var properties = await storageFile.GetBasicPropertiesAsync();
            var specificProperties = await properties.RetrievePropertiesAsync(MetaDataProperties.Select(pair => pair.Value).ToList());
            Dictionary<SystemProperty, string> dictionary = new Dictionary<SystemProperty, string>();
            foreach (var property in specificProperties)
            {
                if (MetaDataProperties.ContainsValue(property.Key) && property.Value != null)
                {
                    dictionary.Add((SystemProperty)MetaDataProperties.Single(valuePair => valuePair.Value == property.Key)
                        .Key, property.Value.ToString());
                }
            }
            return dictionary;
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ff521709(v=vs.85).aspx
        /// </summary>
        public Dictionary<int, string> MetaDataProperties => new Dictionary<int, string>
        {
            {(int) SystemProperty.CameraManufacturer, "System.Photo.CameraManufacturer"},
            {(int) SystemProperty.CameraModel, "System.Photo.CameraModel"},
            {(int) SystemProperty.FStop, "System.Photo.FNumber"},
            {(int) SystemProperty.FStopNumerator, "System.Photo.FNumberNumerator"},
            {(int) SystemProperty.FStopDenominator, "System.Photo.FNumberDenominator"},
            {(int) SystemProperty.ExposureTime, "System.Photo.ExposureTime"},
            {(int) SystemProperty.ISOSpeed, "System.Photo.ISOSpeed"},
            {(int) SystemProperty.FocalLength, "System.Photo.FocalLength"},
            {(int) SystemProperty.ShutterSpeedNumerator, "System.Photo.ShutterSpeedNumerator"},
            {(int) SystemProperty.ShutterSpeedDenominator, "System.Photo.ShutterSpeedDenominator"},
            {(int) SystemProperty.Aperture, "System.Photo.Aperture"},
            {(int) SystemProperty.LensModel, "System.Photo.LensModel"},
            {(int) SystemProperty.LensManufacturer, "System.Photo.LensManufacturer"},
            {(int) SystemProperty.Flash, "System.Photo.Flash"},
            {(int) SystemProperty.ExposureBias, "System.Photo.ExposureBias"},
        };
    }

    public enum SystemProperty
    {
        CameraManufacturer = 0,
        CameraModel = 1,
        FStopNumerator = 2,
        FStopDenominator = 3,
        ExposureTime = 4,
        FStop,
        ISOSpeed,
        FocalLength,
        ShutterSpeedNumerator,
        ShutterSpeedDenominator,
        Aperture,
        LensModel,
        LensManufacturer,
        Flash,
        ExposureBias
    }
}