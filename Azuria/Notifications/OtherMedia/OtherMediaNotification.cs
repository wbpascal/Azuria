namespace Azuria.Notifications.OtherMedia
{
    /// <summary>
    /// </summary>
    public class OtherMediaNotification : INotification
    {
        internal OtherMediaNotification(MediaNotification mediaNotification)
        {
            this.MediaNotification = mediaNotification;
            this.NotificationId = mediaNotification.NotificationId;
            this.NotificationType = OtherMediaType.Media;
            this.Senpai = mediaNotification.Senpai;
        }

        internal OtherMediaNotification(string message, int notificationId, Senpai senpai)
        {
            this.Message = message;
            this.NotificationId = notificationId;
            this.Senpai = senpai;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public MediaNotification MediaNotification { get; }

        /// <summary>
        /// </summary>
        public string Message { get; }

        /// <inheritdoc />
        public int NotificationId { get; }

        string INotification.NotificationId => this.NotificationId.ToString();

        /// <summary>
        /// </summary>
        public OtherMediaType NotificationType { get; }

        /// <inheritdoc />
        public Senpai Senpai { get; }

        #endregion
    }
}