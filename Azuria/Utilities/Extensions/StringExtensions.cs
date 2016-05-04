﻿using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class StringExtensions
    {
        #region

        [NotNull]
        internal static List<string> GetTagContents([NotNull] this string source, [NotNull] string startTag,
            [NotNull] string endTag)
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

        internal static DateTime ToDateTime([NotNull] this string stringToFormat, [NotNull] string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact(
                stringToFormat,
                format,
                CultureInfo.InvariantCulture);
        }

        #endregion
    }
}