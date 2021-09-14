namespace AtaLib.StringExtensions
{
    public static class stringExtensions
    {
        /// <summary>
        /// Centers a string using padding on both sides
        /// </summary>
        /// <param name="source">The string being padded</param>
        /// <param name="length">The total string length, including padding</param>
        /// <param name="padchar">The character to pad with</param>
        /// <returns>The padded string</returns>
        public static string Center(this string source, int length, char padchar = ' ')
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft, padchar).PadRight(length, padchar);

        }
    }
}