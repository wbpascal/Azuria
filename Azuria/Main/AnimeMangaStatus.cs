namespace Azuria.Main
{
    /// <summary>
    ///     Represents the airing status of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public enum AnimeMangaStatus
    {
        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> is currently not published.
        /// </summary>
        PreAiring,

        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> is currently airing.
        /// </summary>
        Airing,

        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> was cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> is finished
        /// </summary>
        Completed,

        /// <summary>
        ///     The airing status of the <see cref="Anime" /> or <see cref="Manga" /> is unknown.
        /// </summary>
        Unknown
    }
}