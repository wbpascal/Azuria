using System;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a private message notification.
    /// </summary>
    public class PrivateMessageNotification : INotification
    {
        /// <summary>
        ///     Represents if the private message notification originates from a conference or a single user.
        /// </summary>
        public enum PrivateMessageType
        {
            /// <summary>
            ///     The private message notification originates from a conference.
            /// </summary>
            Conference,

            /// <summary>
            ///     The private message notification originates from a singel user.
            /// </summary>
            User
        }

        internal PrivateMessageNotification(int conId, [NotNull] string userName, DateTime timeStampDate)
            : this("", conId, userName, PrivateMessageType.User, timeStampDate)
        {
        }

        internal PrivateMessageNotification([NotNull] string title, int conId, DateTime timeStampDate)
            : this(title, conId, "", PrivateMessageType.Conference, timeStampDate)
        {
        }

        private PrivateMessageNotification([NotNull] string title, int conId, [NotNull] string userName,
            PrivateMessageType privateMessageType,
            DateTime timeStamp)
        {
            this.Type = NotificationType.PrivateMessage;
            this.Message = privateMessageType.ToString();
            this.MessageTyp = privateMessageType;
            this.ConferenceTitle = title;
            this.UserName = userName;
            this.TimeStamp = timeStamp;
            this.Id = conId;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the message of the notification.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the title of the conference.
        ///     <para>Only available if the notification originates from a conference.</para>
        /// </summary>
        [NotNull]
        public string ConferenceTitle { get; }

        /// <summary>
        ///     Gets the id of the conference.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Gets whether the private message notification originates from a conference or a single user.
        /// </summary>
        public PrivateMessageType MessageTyp { get; }

        /// <summary>
        ///     Gets the date of the private message.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     Gets the username of the sender.
        ///     <para>Only available if the notification originates from a single user.</para>
        /// </summary>
        [NotNull]
        public string UserName { get; }

        #endregion

        #region

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return (this.MessageTyp == PrivateMessageType.Conference ? this.ConferenceTitle : this.UserName) + "\n" +
                   this.TimeStamp;
        }

        #endregion
    }
}