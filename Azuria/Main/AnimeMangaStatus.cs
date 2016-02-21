namespace Azuria.Main
{
    /// <summary>
    ///     Eine Enumeration, die den Status eines <see cref="Anime" /> oder <see cref="Manga" /> darstellt.
    /// </summary>
    public enum AnimeMangaStatus
    {
        /// <summary>
        ///     Der <see cref="Anime" /> oder <see cref="Manga" /> ist noch nicht erschienen.
        /// </summary>
        PreAiring,

        /// <summary>
        ///     Der <see cref="Anime" /> oder <see cref="Manga" /> wird momentan ausgestrahlt.
        /// </summary>
        Airing,

        /// <summary>
        ///     Der <see cref="Anime" /> oder <see cref="Manga" /> wurde abgebrochen.
        /// </summary>
        Canceled,

        /// <summary>
        ///     Der <see cref="Anime" /> oder <see cref="Manga" /> ist abgeschlossen.
        /// </summary>
        Completed,

        /// <summary>
        ///     Der Status des <see cref="Anime" /> oder <see cref="Manga" /> ist unbekannt.
        /// </summary>
        Unknown
    }
}