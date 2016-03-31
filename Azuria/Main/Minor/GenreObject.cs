using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.Main.Minor
{
    /// <summary>
    ///     Eine Klasse, die ein Genre eines <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> darstellt.
    /// </summary>
    public class GenreObject
    {
        /// <summary>
        ///     Eine Enumeration, die einen Genre-Typen darstellt.
        /// </summary>
        public enum GenreType
        {
            /// <summary>
            ///     Stellt das Genre Abenteuer dar.
            /// </summary>
            Adventure,

            /// <summary>
            ///     Stellt das Genre Action dar.
            /// </summary>
            Action,

            /// <summary>
            ///     Stellt das Genre Adult dar.
            /// </summary>
            Adult,

            /// <summary>
            ///     Stellt das Genre Comedy dar.
            /// </summary>
            Comedy,

            /// <summary>
            ///     Stellt das Genre Cyberpunk dar.
            /// </summary>
            Cyberpunk,

            /// <summary>
            ///     Stellt das Genre Drama dar.
            /// </summary>
            Drama,

            /// <summary>
            ///     Stellt das Genre Ecchi dar.
            /// </summary>
            Ecchi,

            /// <summary>
            ///     Stellt das Genre Fantasy dar.
            /// </summary>
            Fantasy,

            /// <summary>
            ///     Stellt das Genre Harem dar.
            /// </summary>
            Harem,

            /// <summary>
            ///     Stellt das Genre Historical dar.
            /// </summary>
            Historical,

            /// <summary>
            ///     Stellt das Genre Horror dar.
            /// </summary>
            Horror,

            /// <summary>
            ///     Stellt das Genre Josei dar.
            /// </summary>
            Josei,

            /// <summary>
            ///     Stellt das Genre Magic dar.
            /// </summary>
            Magic,

            /// <summary>
            ///     Stellt das Genre MartialArt dar.
            /// </summary>
            MartialArt,

            /// <summary>
            ///     Stellt das Genre Mecha dar.
            /// </summary>
            Mecha,

            /// <summary>
            ///     Stellt das Genre Military dar.
            /// </summary>
            Military,

            /// <summary>
            ///     Stellt das Genre Musik dar.
            /// </summary>
            Music,

            /// <summary>
            ///     Stellt das Genre Mystery dar.
            /// </summary>
            Mystery,

            /// <summary>
            ///     Stellt kein Genre dar.
            /// </summary>
            None,

            /// <summary>
            ///     Stellt das Genre Psychological dar.
            /// </summary>
            Psychological,

            /// <summary>
            ///     Stellt das Genre Romance dar.
            /// </summary>
            Romance,

            /// <summary>
            ///     Stellt das Genre School dar.
            /// </summary>
            School,

            /// <summary>
            ///     Stellt das Genre SciFi dar.
            /// </summary>
            SciFi,

            /// <summary>
            ///     Stellt das Genre Seinen dar.
            /// </summary>
            Seinen,

            /// <summary>
            ///     Stellt das Genre Shoujou dar.
            /// </summary>
            Shoujou,

            /// <summary>
            ///     Stellt das Genre ShoujouAi dar.
            /// </summary>
            ShoujouAi,

            /// <summary>
            ///     Stellt das Genre Shounen dar.
            /// </summary>
            Shounen,

            /// <summary>
            ///     Stellt das Genre ShounenAi dar.
            /// </summary>
            ShounenAi,

            /// <summary>
            ///     Stellt das Genre SliceOfLife dar.
            /// </summary>
            SliceOfLife,

            /// <summary>
            ///     Stellt das Genre Splatter dar.
            /// </summary>
            Splatter,

            /// <summary>
            ///     Stellt das Genre Sport dar.
            /// </summary>
            Sport,

            /// <summary>
            ///     Stellt das Genre Superpower dar.
            /// </summary>
            Superpower,

            /// <summary>
            ///     Stellt das Genre Vampire dar.
            /// </summary>
            Vampire,

            /// <summary>
            ///     Stellt das Genre Violence dar.
            /// </summary>
            Violence,

            /// <summary>
            ///     Stellt das Genre Yaoi dar.
            /// </summary>
            Yaoi,

            /// <summary>
            ///     Stellt das Genre Yuri dar.
            /// </summary>
            Yuri
        }

        internal GenreObject([NotNull] string name)
        {
            this.Genre = TypeDictionary.ContainsKey(name) ? TypeDictionary[name] : GenreType.None;
        }

        /// <summary>
        ///     Erzeugt ein neues <see cref="GenreObject">GenreObject</see>-Objekt mit einem angegebenen Genre.
        /// </summary>
        /// <param name="genre">Das Genre, das mit diesem Objekt dargestellt werden soll.</param>
        public GenreObject(GenreType genre)
        {
            this.Genre = genre;
        }

        #region Properties

        /// <summary>
        ///     Gibt das Genre zurück, dass durch dieses Objekt dargestellt wird.
        /// </summary>
        public GenreType Genre { get; set; }

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

        /// <summary>
        ///     Gibt den Link zu dem Wiki-Eintrag des Genre zurück.
        /// </summary>
        [NotNull]
        public Uri WikiLink => new Uri("https://proxer.me/wiki/" + this.Genre);

        #endregion
    }
}