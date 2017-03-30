using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Messenger;
using Azuria.Enums.Messenger;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class MessageDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("conference_id")]
        public int ConferenceId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("action")]
        [JsonConverter(typeof(MessageActionConverter))]
        public MessageAction MessageAction { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("message")]
        public string MessageContent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("message_id")]
        public int MessageId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime MessageTimeStamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("device")]
        public string SenderDevice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("user_id")]
        public int SenderUserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string SenderUsername { get; set; }

        #endregion
    }
}