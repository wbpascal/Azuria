using System;
using Azuria.Community;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    ///     Represents a private message notification.
    /// </summary>
    public class PrivateMessageNotification : INotification
    {
        internal PrivateMessageNotification(Conference conference, DateTime timeStamp)
        {
            this.Conference = conference;
            this.TimeStamp = timeStamp;
            this.NotificationId = this.Conference.Id.ToString() +
                                  this.TimeStamp.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        #region Properties

        /// <summary>
        ///     Gets the conference the private message was recieved from.
        /// </summary>
        public Conference Conference { get; }

        /// <summary>
        ///     Gets the id of the notification.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        ///     Gets the date of the private message.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => NotificationType.PrivateMessage;

        #endregion
    }
}