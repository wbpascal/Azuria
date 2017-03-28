using System;
using System.Collections.Generic;
using System.Text;
using Azuria.Api.Enums.Info;

namespace Azuria.Api.Helpers.Extensions
{
    internal static class LanguageExtensions
    {
        internal static string ToShortString(this Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "en";
                case Language.German:
                    return "de";
                default:
                    return string.Empty;
            }
        }
    }
}
