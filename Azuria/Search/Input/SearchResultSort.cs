namespace Azuria.Search.Input
{
    /// <summary>
    /// </summary>
    public enum SearchResultSort
    {
        /// <summary>
        ///     Sort the results by best match first.
        /// </summary>
        Relevance,

        /// <summary>
        ///     Sort the results by alphabet.
        /// </summary>
        Name,

        /// <summary>
        ///     Sort the results by rating. First the rating count and then by average.
        /// </summary>
        Rating,

        /// <summary>
        ///     Sort the results by amount of clicks.
        /// </summary>
        Clicks,

        /// <summary>
        ///     Sort the results by their chapter or episode count.
        /// </summary>
        Count
    }
}