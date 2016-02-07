using System.Collections.Generic;

namespace Azuria.Main.Minor
{
    /// <summary>
    /// Eine Klasse, die einige Hilfestellungen gibt, um mit der <see cref="Fsk">Fsk-Enumeration</see> zu arbeiten.
    /// </summary>
    public static class FskHelper
    {
        internal static Dictionary<string, Fsk> StringToFskDictionary => new Dictionary<string, Fsk>
        {
            {"fsk0", Fsk.Fsk0},
            {"fsk6", Fsk.Fsk6},
            {"fsk12", Fsk.Fsk12},
            {"fsk16", Fsk.Fsk16},
            {"fsk18", Fsk.Fsk18},
            {"bad_language", Fsk.Schimpfwoerter},
            {"violence", Fsk.Gewalt},
            {"fear", Fsk.Furcht},
            {"sex", Fsk.Sex}
        };

        /// <summary>
        /// Ein Wörterbuch, dass bei der Umwandlung von <see cref="Fsk">Fsk</see> in Strings hilft.
        /// </summary>
        public static Dictionary<Fsk, string> FskToStringDictionary => new Dictionary<Fsk, string>
        {
            {Fsk.Fsk0, "fsk0"},
            {Fsk.Fsk6, "fsk6"},
            {Fsk.Fsk12, "fsk12"},
            {Fsk.Fsk16, "fsk16"},
            {Fsk.Fsk18, "fsk18"},
            {Fsk.Schimpfwoerter, "bad_language"},
            {Fsk.Gewalt, "violence"},
            {Fsk.Furcht, "fear"},
            {Fsk.Sex, "sex"}
        };
    }
}
