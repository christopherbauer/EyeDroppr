using Windows.UI;

namespace EyeDroppr.ViewModels
{
    public struct ColorStatisticsViewModel
    {
        public ColorStatisticsViewModel(Color color, int count)
        {
            Color = color;
            Count = count;
        }
        public Color Color { get; set; }

        public int Count { get; set; }
    }
}