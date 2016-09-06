using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.UserInfo.ControlPanel
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

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Task<ProxerResult> DeleteEntry()
        {
            return this.UserControlPanel.DeleteCommentVote(this.VoteId);
        }

        #endregion
    }
}