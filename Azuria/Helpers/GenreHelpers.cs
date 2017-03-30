using System.Collections.Generic;
using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class GenreHelpers
    {
        #region Properties

        internal static Dictionary<string, Genre> StringToGenreDictionary => new Dictionary<string, Genre>
        {
            {"Abenteuer", Genre.Adventure},
            {"Action", Genre.Action},
            {"Adult", Genre.Adult},
            {"Comedy", Genre.Comedy},
            {"Cyberpunk", Genre.Cyberpunk},
            {"Drama", Genre.Drama},
            {"Ecchi", Genre.Ecchi},
            {"Fantasy", Genre.Fantasy},
            {"Harem", Genre.Harem},
            {"Historical", Genre.Historical},
            {"Horror", Genre.Horror},
            {"Josei", Genre.Josei},
            {"Magic", Genre.Magic},
            {"Martial-Art", Genre.MartialArt},
            {"Mecha", Genre.Mecha},
            {"Military", Genre.Military},
            {"Musik", Genre.Music},
            {"Mystery", Genre.Mystery},
            {"Psychological", Genre.Psychological},
            {"Romance", Genre.Romance},
            {"School", Genre.School},
            {"SciFi", Genre.SciFi},
            {"Seinen", Genre.Seinen},
            {"Shoujou", Genre.Shoujou},
            {"Shoujou-Ai", Genre.ShoujouAi},
            {"Shounen", Genre.Shounen},
            {"Shounen-Ai", Genre.ShounenAi},
            {"Slice_of_Life", Genre.SliceOfLife},
            {"Splatter", Genre.Splatter},
            {"Sport", Genre.Sport},
            {"Superpower", Genre.Superpower},
            {"Vampire", Genre.Vampire},
            {"Violence", Genre.Violence},
            {"Yaoi", Genre.Yaoi},
            {"Yuri", Genre.Yuri}
        };

        #endregion
    }
}