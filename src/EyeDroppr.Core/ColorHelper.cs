namespace Core
{
    using Windows.UI;

    public static class ColorHelper
    {
        public static bool IsInRange(this Color sourceColor, Color color, int fuzziness)
        {
            if (sourceColor.A > color.A - fuzziness && sourceColor.A < color.A + fuzziness)
            {
                if (sourceColor.B > color.B - fuzziness && sourceColor.B < color.B + fuzziness)
                {
                    if (sourceColor.G > color.G - fuzziness && sourceColor.G < color.G + fuzziness)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}