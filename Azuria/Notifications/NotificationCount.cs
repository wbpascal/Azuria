using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications
{
    /// <summary>
    /// </summary>
    public class NotificationCount
    {
        internal NotificationCount(NotificationCountDataModel dataModel)
        {
            this.FriendRequests = dataModel.FriendRequests;
            this.Messages = dataModel.PrivateMessages;
            this.News = dataModel.News;
            this.OtherMedia = dataModel.OtherMedia;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int FriendRequests { get; }

        /// <summary>
        /// </summary>
        public int Messages { get; }

        /// <summary>
        /// </summary>
        public int News { get; }

        /// <summary>
        /// </summary>
        public int OtherMedia { get; }

        #endregion
    }
}