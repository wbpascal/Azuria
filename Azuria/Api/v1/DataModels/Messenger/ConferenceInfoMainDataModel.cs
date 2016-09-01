using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    internal class ConferenceInfoMainDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("timestamp_start")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime FirstMessageTimeStamp { get; set; }

        [JsonProperty("timestamp_end")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime LastMessageTimeStamp { get; set; }

        [JsonProperty("leader")]
        internal int LeaderUserId { get; set; }

        [JsonProperty("topic")]
        internal string Title { get; set; }

        [JsonProperty("count")]
        internal int UserCount { get; set; }

        #endregion
    }
}