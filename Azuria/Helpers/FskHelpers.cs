using System.Collections.Generic;
using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class FskHelpers
    {
        #region Properties

        internal static Dictionary<Fsk, string> FskToStringDictionary => new Dictionary<Fsk, string>
        {
            {Fsk.Fsk0, "fsk0"},
            {Fsk.Fsk6, "fsk6"},
            {Fsk.Fsk12, "fsk12"},
            {Fsk.Fsk16, "fsk16"},
            {Fsk.Fsk18, "fsk18"},
            {Fsk.BadWords, "bad_language"},
            {Fsk.Violence, "violence"},
            {Fsk.Fear, "fear"},
            {Fsk.Sex, "sex"},
            {Fsk.Unknown, "unknown"}
        };

        internal static Dictionary<string, Fsk> StringToFskDictionary => new Dictionary<string, Fsk>
        {
            {"fsk0", Fsk.Fsk0},
            {"fsk6", Fsk.Fsk6},
            {"fsk12", Fsk.Fsk12},
            {"fsk16", Fsk.Fsk16},
            {"fsk18", Fsk.Fsk18},
            {"bad_language", Fsk.BadWords},
            {"violence", Fsk.Violence},
            {"fear", Fsk.Fear},
            {"sex", Fsk.Sex},
            {"unknown", Fsk.Unknown}
        };

        #endregion
    }
}