using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents a class that describes the genre of an <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see>.
    /// </summary>
    public class GenreObject
    {
        internal GenreObject([NotNull] string name)
        {
            this.Genre = TypeDictionary.ContainsKey(name) ? TypeDictionary[name] : GenreType.None;
        }

        /// <summary>
        ///     Initialises a <see cref="GenreObject">GenreObject</see> with a specified genre.
        /// </summary>
        /// <param name="genre">The genre that is represented by the object.</param>
        public GenreObject(GenreType genre)
        {
            this.Genre = genre;
        }

        #region Properties

        /// <summary>
        ///     Gets the genre this object is associated with.
        /// </summary>
        public GenreType Genre { get; }

        [NotNull]
        internal static Dictionary<string, GenreType> TypeDictionary => new Dictionary<string, GenreType>
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
            {"Slice_Of_Life", GenreType.SliceOfLife},
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