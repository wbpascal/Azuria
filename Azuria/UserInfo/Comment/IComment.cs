using System.Collections.Generic;
using Azuria.Media;

namespace Azuria.UserInfo.Comment
{
    /// <summary>
    /// </summary>
    public interface IComment
    {
        #region Properties

        /// <summary>
        /// Gets the author of this comment.
        /// </summary>
        User Author { get; }

        /// <summary>
        /// Gets the content of this comment.
        /// </summary>
        string Content { get; }

        /// <summary>
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> this comment is for.
        /// </summary>
        IMediaObject MediaObject { get; }

        /// <summary>
        /// </summary>
        int Progress { get; }

        /// <summary>
        /// Gets the category the author has put his progress of the <see cref="Anime" /> or <see cref="Manga" /> in.
        /// </summary>
        MediaProgressState ProgressState { get; }

        /// <summary>
        /// Gets the overall rating the <see cref="Author" /> gave. Returns -1 if no rating was found.
        /// </summary>
        int Rating { get; }

        /// <summary>
        /// Gets the rating of all subcategories.
        /// </summary>
        Dictionary<RatingCategory, int> SubRatings { get; }

        /// <summary>
        /// </summary>
        int Upvotes { get; }

        #endregion
    }
}