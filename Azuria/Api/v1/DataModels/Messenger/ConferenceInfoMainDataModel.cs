using System;
using Azuria.Api.v1.Converter;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class ConferenceInfoMainDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("timestamp_start")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime FirstMessageTimeStamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp_end")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime LastMessageTimeStamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("leader")]
        public int LeaderUserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("topic")]
        public string Title { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int UserCount { get; set; }
    }
}