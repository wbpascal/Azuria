using System.Collections.Generic;

namespace Azuria.Media.Properties
{
    /// <summary>
    /// Represents a class which aims to help working with the <see cref="FskType">Fsk-enumeration</see>.
    /// </summary>
    public static class GenreHelper
    {
        #region Properties

        internal static Dictionary<string, GenreType> StringToGenreDictionary => new Dictionary<string, GenreType>
        {
            {"Abenteuer", GenreType.Adventure},
            {"Action", GenreType.Action},
            {"Adult", GenreType.Adult},
            {"Comedy", GenreType.Comedy},
            {"Cyberpunk", GenreType.Cyberpunk},
            {"Drama", GenreType.Drama},
            {"Ecchi", GenreType.Ecchi},
            {"Fantasy", GenreType.Fantasy},
            {"Harem", GenreType.Harem},
            {"Historical", GenreType.Historical},
            {"Horror", GenreType.Horror},
            {"Josei", GenreType.Josei},
            {"Magic", GenreType.Magic},
            {"Martial-Art", GenreType.MartialArt},
            {"Mecha", GenreType.Mecha},
            {"Military", GenreType.Military},
            {"Musik", GenreType.Music},
            {"Mystery", GenreType.Mystery},
            {"Psychological", GenreType.Psychological},
            {"Romance", GenreType.Romance},
            {"School", GenreType.School},
            {"SciFi", GenreType.SciFi},
            {"Seinen", GenreType.Seinen},
            {"Shoujou", GenreType.Shoujou},
            {"Shoujou-Ai", GenreType.ShoujouAi},
            {"Shounen", GenreType.Shounen},
            {"Shounen-Ai", GenreType.ShounenAi},
            {"Slice_of_Life", GenreType.SliceOfLife},
            {"Splatter", GenreType.Splatter},
            {"Sport", GenreType.Sport},
            {"Superpower", GenreType.Superpower},
            {"Vampire", GenreType.Vampire},
            {"Violence", GenreType.Violence},
            {"Yaoi", GenreType.Yaoi},
            {"Yuri", GenreType.Yuri}
        };

        #endregion
    }
}