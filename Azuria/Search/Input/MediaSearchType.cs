namespace Azuria.Search.Input
{
    /// <summary>
    /// An enumeration which describes the type of the anime or manga that
    /// is being searched for.
    /// </summary>
    public enum MediaSearchType
    {
        /// <summary>
        /// Search for everything. Excluding H-Content.
        /// </summary>
        All,

        /// <summary>
        /// Search for everything including H-Content.
        /// </summary>
        All18,

        /// <summary>
        /// Search for every anime type. Excluding H-Content.
        /// </summary>
        AllAnime,

        /// <summary>
        /// Search only for Hentai.
        /// </summary>
        Hentai,

        /// <summary>
        /// Search only for animeseries.
        /// </summary>
        Animeseries,

        /// <summary>
        /// Search only for OVAs.
        /// </summary>
        Ova,

        /// <summary>
        /// Search only for movies.
        /// </summary>
        Movie,

        /// <summary>
        /// Search for every manga type. Excluding H-Content.
        /// </summary>
        AllManga,

        /// <summary>
        /// Search only for H-Manga.
        /// </summary>
        HManga,

        /// <summary>
        /// Search only for Doujin.
        /// </summary>
        Doujin,

        /// <summary>
        /// Search only for mangaseries.
        /// </summary>
        Mangaseries,

        /// <summary>
        /// Search only for one-shot manga.
        /// </summary>
        OneShot
    }
}