namespace EyeDroppr
{
    using System;

    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }
            var color = (Color)value;

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}