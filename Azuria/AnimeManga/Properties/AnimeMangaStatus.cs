namespace Azuria.AnimeManga.Properties
{
    /// <summary>
    ///     Represents the airing status of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public enum AnimeMangaStatus
    {
        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> is currently not published.
        /// </summary>
        PreAiring = 0,

        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> is currently airing.
        /// </summary>
        Airing = 2,

        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> was cancelled
        /// </summary>
        Cancelled = 3,

        /// <summary>
        ///     The <see cref="Anime" /> or <see cref="Manga" /> is finished
        /// </summary>
        Completed = 1,

        /// <summary>
        ///     The airing status of the <see cref="Anime" /> or <see cref="Manga" /> is unknown.
        /// </summary>
        Unknown
    }
}