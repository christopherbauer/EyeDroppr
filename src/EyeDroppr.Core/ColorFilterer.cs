namespace Core
{
    using System.Collections.Generic;
    using System.Linq;

    using Windows.UI;

    /// <summary>
    /// The color filterer.
    /// </summary>
    public class ColorFilterer
    {
        /// <summary>
        /// The get top colors.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <param name="n">
        /// The n.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary{TKey,TValue}"/>.
        /// </returns>
        public Dictionary<Color, int> GetTopColors(Dictionary<Color, int> dictionary, int n)
        {
            return dictionary.OrderByDescending(pair => pair.Value).Take(n).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

    }
}