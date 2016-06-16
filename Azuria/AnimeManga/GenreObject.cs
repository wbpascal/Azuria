using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents a class that describes the genre of an <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see>.
    /// </summary>
    public class GenreObject
    {
        /// <summary>
        ///     Represents an enumeration which contains every possible genre for an <see cref="Anime">Anime</see> or
        ///     <see cref="Manga">Manga</see>.
        /// </summary>
        public enum GenreType
        {
            /// <summary>
            ///     Represents the Adventure genre.
            /// </summary>
            Adventure,

            /// <summary>
            ///     Represents the Action genre.
            /// </summary>
            Action,

            /// <summary>
            ///     Represents the Adult genre.
            /// </summary>
            Adult,

            /// <summary>
            ///     Represents the Comedy genre.
            /// </summary>
            Comedy,

            /// <summary>
            ///     Represents the Cyberpunk genre.
            /// </summary>
            Cyberpunk,

            /// <summary>
            ///     Represents the Drama genre.
            /// </summary>
            Drama,

            /// <summary>
            ///     Represents the Ecchi genre.
            /// </summary>
            Ecchi,

            /// <summary>
            ///     Represents the Fantasy genre.
            /// </summary>
            Fantasy,

            /// <summary>
            ///     Represents the Harem genre.
            /// </summary>
            Harem,

            /// <summary>
            ///     Represents the Historical genre.
            /// </summary>
            Historical,

            /// <summary>
            ///     Represents the Horror genre.
            /// </summary>
            Horror,

            /// <summary>
            ///     Represents the Josei genre.
            /// </summary>
            Josei,

            /// <summary>
            ///     Represents the Magic genre.
            /// </summary>
            Magic,

            /// <summary>
            ///     Represents the MartialArt genre.
            /// </summary>
            MartialArt,

            /// <summary>
            ///     Represents the MartialArt genre.
            /// </summary>
            Mecha,

            /// <summary>
            ///     Represents the Military genre.
            /// </summary>
            Military,

            /// <summary>
            ///     Represents the Music genre.
            /// </summary>
            Music,

            /// <summary>
            ///     Represents the Mystery genre.
            /// </summary>
            Mystery,

            /// <summary>
            ///     Represents no genre.
            /// </summary>
            None,

            /// <summary>
            ///     Represents the Psychological genre.
            /// </summary>
            Psychological,

            /// <summary>
            ///     Represents the Romance genre.
            /// </summary>
            Romance,

            /// <summary>
            ///     Represents the School genre.
            /// </summary>
            School,

            /// <summary>
            ///     Represents the SciFi genre.
            /// </summary>
            SciFi,

            /// <summary>
            ///     Represents the Seinen genre.
            /// </summary>
            Seinen,

            /// <summary>
            ///     Represents the Shoujou genre.
            /// </summary>
            Shoujou,

            /// <summary>
            ///     Represents the ShoujouAi genre.
            /// </summary>
            ShoujouAi,

            /// <summary>
            ///     Represents the Shounen genre.
            /// </summary>
            Shounen,

            /// <summary>
            ///     Represents the ShounenAi genre.
            /// </summary>
            ShounenAi,

            /// <summary>
            ///     Represents the SliceOfLife genre.
            /// </summary>
            SliceOfLife,

            /// <summary>
            ///     Represents the Splatter genre.
            /// </summary>
            Splatter,

            /// <summary>
            ///     Represents the Sport genre.
            /// </summary>
            Sport,

            /// <summary>
            ///     Represents the Superpower genre.
            /// </summary>
            Superpower,

            /// <summary>
            ///     Represents the Vampire genre.
            /// </summary>
            Vampire,

            /// <summary>
            ///     Represents the Violence genre.
            /// </summary>
            Violence,

            /// <summary>
            ///     Represents the Yaoi genre.
            /// </summary>
            Yaoi,

            /// <summary>
            ///     Represents the Yuri genre.
            /// </summary>
            Yuri
        }

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