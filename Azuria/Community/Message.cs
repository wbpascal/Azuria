using System;
using Azuria.Utilities;
using JetBrains.Annotations;

namespace Azuria.Community
{
    /// <summary>
    ///     Represents a single message of a <see cref="Conference" />
    /// </summary>
    public class Message
    {
        /// <summary>
        ///     The action of the message.
        /// </summary>
        public enum Action
        {
            /// <summary>
            ///     Normal message, only text content.
            /// </summary>
            NoAction,

            /// <summary>
            ///     A user was added to the conference.
            /// </summary>
            AddUser,

            /// <summary>
            ///     A user was removed from the conference.
            /// </summary>
            RemoveUser,

            /// <summary>
            ///     The leader of the conference was changed.
            /// </summary>
            SetLeader,

            /// <summary>
            ///     The topic of the conference was changed.
            /// </summary>
            SetTopic,

            /// <summary>
            ///     A message was returned directly by the system. Happens most of the times if the user issued a command.
            /// </summary>
            GetAction
        }

        internal Message([NotNull] User.User sender, int mid, [NotNull] string nachricht, int unix, Action action)
            : this(sender, mid, nachricht, Utility.UnixTimeStampToDateTime(unix), action)
        {
        }

        internal Message([NotNull] User.User sender, int mid, [NotNull] string nachricht, DateTime date,
            Action action)
        {
            this.Sender = sender;
            this.MessageId = mid;
            this.Content = nachricht;
            this.TimeStamp = date;
            this.MessageAction = action;
        }

        #region Properties

        /// <summary>
        ///     Gets the message content.
        /// </summary>
        [NotNull]
        public string Content { get; private set; }

        /// <summary>
        ///     Gets the action of the message.
        /// </summary>
        public Action MessageAction { get; }

        /// <summary>
        ///     Gets the Id of the current message.
        /// </summary>
        public int MessageId { get; }

        /// <summary>
        ///     Gets the sender of the current message.
        /// </summary>
        [NotNull]
        public User.User Sender { get; private set; }

        /// <summary>
        ///     Gets the timestamp of the current message.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        #endregion
    }
}