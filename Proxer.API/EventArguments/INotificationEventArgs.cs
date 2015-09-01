namespace Proxer.API.EventArguments
{
    /// <summary>
    /// </summary>
    public interface INotificationEventArgs
    {
        #region Properties

        /// <summary>
        /// </summary>
        int NotificationCount { get; }

        /// <summary>
        /// </summary>
        NotificationEventArgsType Type { get; }

        #endregion
    }
}