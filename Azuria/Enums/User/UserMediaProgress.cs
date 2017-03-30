namespace Azuria.Enums.User
{
    /// <summary>
    /// Represents the category a user has categorised his progress of an anime or manga in.
    /// </summary>
    public enum MediaProgressState
    {
        /// <summary>
        /// The user finished the anime or manga.
        /// </summary>
        Finished = 0,

        /// <summary>
        /// The user is currently watching the anime or manga.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The user plans to see the anime or manga.
        /// </summary>
        Planned = 2,

        /// <summary>
        /// The user has aborted watching the anime or manga.
        /// </summary>
        Aborted = 3,

        /// <summary>
        /// The category is unknown.
        /// </summary>
        Unknown = 4
    }
}