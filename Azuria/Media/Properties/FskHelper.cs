using System.Collections.Generic;

namespace Azuria.Media.Properties
{
    /// <summary>
    /// Represents a class which aims to help working with the <see cref="FskType">Fsk-enumeration</see>.
    /// </summary>
    public static class FskHelper
    {
        #region Properties

        internal static Dictionary<FskType, string> FskToStringDictionary => new Dictionary<FskType, string>
        {
            {FskType.Fsk0, "fsk0"},
            {FskType.Fsk6, "fsk6"},
            {FskType.Fsk12, "fsk12"},
            {FskType.Fsk16, "fsk16"},
            {FskType.Fsk18, "fsk18"},
            {FskType.BadWords, "bad_language"},
            {FskType.Violence, "violence"},
            {FskType.Fear, "fear"},
            {FskType.Sex, "sex"},
            {FskType.Unknown, "unknown"}
        };

        internal static Dictionary<string, FskType> StringToFskDictionary => new Dictionary<string, FskType>
        {
            {"fsk0", FskType.Fsk0},
            {"fsk6", FskType.Fsk6},
            {"fsk12", FskType.Fsk12},
            {"fsk16", FskType.Fsk16},
            {"fsk18", FskType.Fsk18},
            {"bad_language", FskType.BadWords},
            {"violence", FskType.Violence},
            {"fear", FskType.Fear},
            {"sex", FskType.Sex},
            {"unknown", FskType.Unknown}
        };

        #endregion
    }
}