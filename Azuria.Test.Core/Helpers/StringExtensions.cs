﻿namespace Azuria.Test.Core.Helpers
{
    internal static class StringExtensions
    {
        #region Methods

        internal static string RemoveIfNotEmpty(this string source, int startIndex, int count)
        {
            return string.IsNullOrEmpty(source) ? source : source.Remove(startIndex, count);
        }

        #endregion
    }
}