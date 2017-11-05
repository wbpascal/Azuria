using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Messenger;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class ConferenceDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int ConferenceId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("image")]
        [JsonConverter(typeof(ImageConverter))]
        public Uri ConferenceImage { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("topic")]
        public string ConferenceTitle { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("topic_custom")]
        public string ConferenceUserTitle { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("group")]
        public bool IsConferenceGroup { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("read")]
        public bool IsLastMessageRead { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp_end")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime LastMessageTimeStamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("read_mid")]
        public int LastReadMessageId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int ParticipantsCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("read_count")]
        public int UnreadMessagesCount { get; set; }
    }
}