using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Messenger;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    internal class ConferenceDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("id")]
        internal int ConferenceId { get; set; }

        [JsonProperty("image")]
        [JsonConverter(typeof(ImageConverter))]
        internal Uri ConferenceImage { get; set; }

        [JsonProperty("topic")]
        internal string ConferenceTitle { get; set; }

        [JsonProperty("topic_custom")]
        internal string ConferenceUserTitle { get; set; }

        [JsonProperty("read")]
        internal bool IsLastMessageRead { get; set; }

        [JsonProperty("group")]
        internal bool IsConferenceGroup { get; set; }

        [JsonProperty("timestamp_end")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime LastMessageTimeStamp { get; set; }

        [JsonProperty("read_mid")]
        internal int LastReadMessageId { get; set; }

        [JsonProperty("count")]
        internal int ParticipantsCount { get; set; }

        [JsonProperty("read_count")]
        internal int UnreadMessagesCount { get; set; }

        #endregion
    }
}