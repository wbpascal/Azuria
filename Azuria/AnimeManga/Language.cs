namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents an enumeration wich describes the language of an <see cref="Anime">Anime</see> or
    ///     <see cref="Manga">Manga</see>.
    /// </summary>
    public enum Language
    {
        /// <summary>
        ///     The language of the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is german.
        /// </summary>
        German = 0,

        /// <summary>
        ///     The language of the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is english.
        /// </summary>
        English = 1,

        /// <summary>
        ///     The language of the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is unknown.
        /// </summary>
        Unkown = 6
    }
}