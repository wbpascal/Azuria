using Azuria.Api.v1.DataModels.Ucp;
using Azuria.User.ControlPanel;

namespace Azuria.User.Comment
{
    /// <summary>
    /// </summary>
    public class CommentVote
    {
        internal CommentVote(VoteDataModel dataModel, UserControlPanel userControlPanel)
        {
            this.AnimeMangaName = dataModel.EntryName;
            this.Author = new User(dataModel.Username, dataModel.UserId);
            this.CommentContent = dataModel.CommentContent;
            this.CommentId = dataModel.CommentId;
            this.Rating = dataModel.Rating;
            this.UserControlPanel = userControlPanel;
            this.VoteId = dataModel.VoteId;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public string AnimeMangaName { get; }

        /// <summary>
        /// </summary>
        public User Author { get; }

        /// <summary>
        /// </summary>
        public string CommentContent { get; }

        /// <summary>
        /// </summary>
        public int CommentId { get; }

        /// <summary>
        /// </summary>
        public int Rating { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        /// <summary>
        /// </summary>
        public int VoteId { get; }

        #endregion
    }
}