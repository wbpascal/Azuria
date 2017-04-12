using System;
using System.Globalization;

namespace Azuria.Helpers.Extensions
{
    internal static class StringExtensions
    {
        #region Methods

        internal static string RemoveIfNotEmpty(this string source, int startIndex, int count)
        {
            return string.IsNullOrEmpty(source) ? source : source.Remove(startIndex, count);
        }

        internal static DateTime ToDateTime(this string stringToFormat, string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact(stringToFormat, format, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}