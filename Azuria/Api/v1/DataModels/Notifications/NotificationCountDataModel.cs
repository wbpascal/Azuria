namespace Azuria.Api.v1.DataModels.Notifications
{
    /// <summary>
    /// </summary>
    public class NotificationCountDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        public int FriendRequests { get; set; }

        /// <summary>
        /// </summary>
        public int News { get; set; }

        /// <summary>
        /// </summary>
        public int OtherMedia { get; set; }

        /// <summary>
        /// </summary>
        public int PrivateMessages { get; set; }
    }
}