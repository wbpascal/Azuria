namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public interface INotificationObject
    {
        #region Properties

        /// <summary>
        /// </summary>
        /// <returns></returns>
        string Message { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        NotificationObjectType Type { get; }

        #endregion
    }
}