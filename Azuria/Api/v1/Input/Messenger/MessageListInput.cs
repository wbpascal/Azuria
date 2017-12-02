using Azuria.Api.v1.Input.Converter;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MessageListInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the conference from where the messages should be fetched. 
        /// Optional, if omitted, null or 0 a specified amount (see messenger constants) of the most recent 
        /// messages, regardless of the conference they are from, will be returned
        /// </summary>
        [InputData("conference_id", Optional = true)]
        public int? ConferenceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the conference will be marked as read after returning the messages. 
        /// This value will be only be used if the messages are returned from a specific conference (<see cref="ConferenceId"/> is not null or 0). 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("read", Converter = typeof(ToLowerConverter), Optional = true)]
        public bool? MarkRead { get; set; }

        /// <summary>
        /// Gets or sets the id of the oldest message that will be returned. 
        /// Optional, if omitted (or null) a specified amount (see messenger constants) of the most recent messages of the conference will be returned.
        /// </summary>
        [InputData("message_id", Optional = true)]
        public int? MessageId { get; set; }
    }
}