using System;
using System.Collections.Generic;

namespace Azuria.Example.Utilities
{
    public static class Utility
    {
        #region

        internal static List<string> GetTagContents(string source, string startTag, string endTag)
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

        #endregion
    }
}