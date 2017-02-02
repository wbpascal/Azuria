using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    public interface IBookmark
    {
        #region Properties

        /// <summary>
        /// Gets the id of this bookmark.
        /// </summary>
        int BookmarkId { get; }

        /// <summary>
        /// Gets the <see cref="Episode" /> or <see cref="Chapter" /> the user has bookmarked.
        /// </summary>
        IMediaContent MediaContentObject { get; }

        /// <summary>
        /// </summary>
        UserControlPanel UserControlPanel { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the entry from the User-Control-Panel.
        /// </summary>
        /// <returns>If the action was successfull.</returns>
        Task<IProxerResult> Delete();

        #endregion
    }
}