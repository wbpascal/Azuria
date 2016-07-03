using System;
using Azuria.Community.Conference;
using JetBrains.Annotations;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    ///     Represents a private message notification.
    /// </summary>
    public class PrivateMessageNotification : INotification
    {
        internal PrivateMessageNotification([NotNull] Conference conference, DateTime timeStamp)
        {
            this.Conference = conference;
            this.TimeStamp = timeStamp;
        }

        #region Inherited

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => NotificationType.PrivateMessage;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the conference the private message was recieved from.
        /// </summary>
        public Conference Conference { get; }

        /// <summary>
        ///     Gets the date of the private message.
        /// </summary>
        public DateTime TimeStamp { get; }

        #endregion
    }
}