using Azuria.Media;
using Azuria.UserInfo.Comment;

namespace Azuria.UserInfo
{
    /// <summary>
    /// </summary>
    public interface IUserProfileEntry
    {
        #region Properties

        /// <summary>
        /// </summary>
        IComment Comment { get; }

        /// <summary>
        /// </summary>
        IMediaObject MediaObject { get; }

        /// <summary>
        /// </summary>
        User User { get; set; }

        #endregion
    }
}