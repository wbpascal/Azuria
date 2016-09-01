using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Messenger;
using Azuria.Community;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    internal class MessageDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("conference_id")]
        internal int ConferenceId { get; set; }

        [JsonProperty("action")]
        [JsonConverter(typeof(MessageActionConverter))]
        internal MessageAction MessageAction { get; set; }

        [JsonProperty("message")]
        internal string MessageContent { get; set; }

        [JsonProperty("message_id")]
        internal int MessageId { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime MessageTimeStamp { get; set; }

        [JsonProperty("device")]
        internal string SenderDevice { get; set; }

        [JsonProperty("user_id")]
        internal int SenderUserId { get; set; }

        [JsonProperty("username")]
        internal string SenderUsername { get; set; }

        #endregion
    }
}