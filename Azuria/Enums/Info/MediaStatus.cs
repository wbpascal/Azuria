namespace Azuria.Enums.Info
{
    /// <summary>
    /// Represents the airing status of an anime or manga.
    /// </summary>
    public enum MediaStatus
    {
        /// <summary>
        /// The anime or manga is currently not published.
        /// </summary>
        PreAiring = 0,

        /// <summary>
        /// The anime or manga is currently airing.
        /// </summary>
        Airing = 2,

        /// <summary>
        /// The anime or manga was cancelled.
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// The anime or manga is completed.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// The anime or manga is completed but not fully translated.
        /// </summary>
        CompletedNotFullyTranslated = 4,

        /// <summary>
        /// The airing status of the anime or manga is unknown.
        /// </summary>
        Unknown
    }
}