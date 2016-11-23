using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Azuria.Utilities.Extensions
{
    internal static class StringExtensions
    {
        #region Methods

        internal static bool ContainsOne(this string source, params string[] contains)
        {
            return contains.Any(source.Contains);
        }

        internal static List<string> GetTagContents(this string source, string startTag, string endTag)
        {
            List<string> stringsFound = new List<string>();
            int index = source.IndexOf(startTag, StringComparison.Ordinal) + startTag.Length;

            try
            {
                while (index != startTag.Length - 1)
                {
                    stringsFound.Add(source.Substring(index,
                        source.IndexOf(endTag, index, StringComparison.Ordinal) - index));
                    index = source.IndexOf(startTag, index, StringComparison.Ordinal) + startTag.Length;
                }
            }
            catch
            {
                // ignored
            }
            return stringsFound;
        }

        internal static DateTime ToDateTime(this string stringToFormat, string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact(
                stringToFormat,
                format,
                CultureInfo.InvariantCulture);
        }

        #endregion
    }
}