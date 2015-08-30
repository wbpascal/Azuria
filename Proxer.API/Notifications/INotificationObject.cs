namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public interface INotificationObject
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        string Message { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        NotificationObjectType Type { get; }
    }

    /// <summary>
    /// </summary>
    public enum NotificationObjectType
    {
        /// <summary>
        /// </summary>
        FriendRequest,

        /// <summary>
        /// </summary>
        News,

        /// <summary>
        /// </summary>
        PrivateMessage,

        /// <summary>
        /// </summary>
        AnimeManga
    };
}