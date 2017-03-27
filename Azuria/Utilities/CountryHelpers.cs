using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Media.Properties;

namespace Azuria.Utilities
{
    internal static class CountryHelpers
    {
        public static Country GetCountry(this Language lang)
        {
            switch (lang)
            {
                case Language.English:
                    return Country.EnglandUnitedStates;
                case Language.German:
                    return Country.Germany;
                default:
                    return Country.Miscellaneous;
            }
        }
        
        public static string ToShortString(this Country? country)
        {
            switch (country)
            {
                case Country.EnglandUnitedStates:
                    return "us";
                case Country.Germany:
                    return "de";
                case Country.Japan:
                    return "jp";
                case Country.Miscellaneous:
                    return "misc";
                default:
                    return string.Empty;
            }
        }
    }
}
