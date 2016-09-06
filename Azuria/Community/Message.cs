using System;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.UserInfo;

namespace Azuria.Community
{
    /// <summary>
    ///     Represents a single message of a <see cref="Conference" />
    /// </summary>
    public class Message
    {
        internal Message(MessageDataModel dataModel, int conferenceId)
        {
            this.ConferenceId = conferenceId;
            this.Content = dataModel.MessageContent;
            this.Device = dataModel.SenderDevice;
            this.Action = dataModel.MessageAction;
            this.MessageId = dataModel.MessageId;
            this.Sender = new User(dataModel.SenderUsername, dataModel.SenderUserId);
            this.TimeStamp = dataModel.MessageTimeStamp;
        }

        #region Properties

        /// <summary>
        ///     Gets the action of the message.
        /// </summary>
        public MessageAction Action { get; }

        /// <summary>
        /// </summary>
        public int ConferenceId { get; }

        /// <summary>
        ///     Gets the message content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// </summary>
        public string Device { get; }

        /// <summary>
        ///     Gets the Id of the current message.
        /// </summary>
        public int MessageId { get; }

        /// <summary>
        ///     Gets the sender of the current message.
        /// </summary>
        public User Sender { get; }

        /// <summary>
        ///     Gets the timestamp of the current message.
        /// </summary>
        public DateTime TimeStamp { get; }

        #endregion
    }
}