namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents an enumeration that describes the language of an <see cref="Anime" />.
    /// </summary>
    public enum AnimeLanguage
    {
        /// <summary>
        ///     The Anime contains german subtitles.
        /// </summary>
        GerSub,

        /// <summary>
        ///     The Anime contains english subtitles.
        /// </summary>
        EngSub,

        /// <summary>
        ///     The Anime contains english audio.
        /// </summary>
        EngDub,

        /// <summary>
        ///     The Anime contains german audio.
        /// </summary>
        GerDub,

        /// <summary>
        ///     The language of the Anime is unknown.
        /// </summary>
        Unknown
    }
}