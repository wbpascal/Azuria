namespace Azuria.Api.Enums.Info
{
    /// <summary>
    /// Represents an enumeration that describes the language of an anime.
    /// </summary>
    public enum AnimeLanguage
    {
        /// <summary>
        /// The Anime contains german subtitles.
        /// </summary>
        GerSub = 4,

        /// <summary>
        /// The Anime contains english subtitles.
        /// </summary>
        EngSub = 2,

        /// <summary>
        /// The Anime contains english audio.
        /// </summary>
        EngDub = 3,

        /// <summary>
        /// The Anime contains german audio.
        /// </summary>
        GerDub = 5,

        /// <summary>
        /// The language of the Anime is unknown.
        /// </summary>
        Unknown = 6
    }
}