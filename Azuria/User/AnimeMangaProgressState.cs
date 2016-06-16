using Azuria.AnimeManga;

namespace Azuria.User
{
    /// <summary>
    ///     Represents the category a user has categorised his progress of an <see cref="Anime" /> or <see cref="Manga" /> in.
    /// </summary>
    public enum AnimeMangaProgressState
    {
        /// <summary>
        ///     The user finished the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        Finished = 0,

        /// <summary>
        ///     The user is currently watching the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        InProgress = 1,

        /// <summary>
        ///     The user plans to see the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        Planned = 2,

        /// <summary>
        ///     The user has aborted watching the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        Aborted = 3,

        /// <summary>
        ///     The category is unknown.
        /// </summary>
        Unknown = 4
    }
}