using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Anime;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Anime
{
    internal class StreamDataModel
    {
        #region Properties

        [JsonProperty("name")]
        internal string HosterFullName { get; set; }

        [JsonProperty("img")]
        internal string HosterImageFileName { get; set; }

        [JsonProperty("htype")]
        internal string HostingType { get; set; }

        [JsonProperty("replace")]
        internal string PlaceholderLink { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StreamPartnerConverter))]
        internal StreamHoster StreamHoster { get; set; }

        [JsonProperty("id")]
        internal int StreamId { get; set; }

        [JsonProperty("tid")]
        internal int? TranslatorId { get; set; }

        [JsonProperty("tname")]
        internal string TranslatorName { get; set; }

        [JsonProperty("uploader")]
        internal int UploaderId { get; set; }

        [JsonProperty("username")]
        internal string UploaderName { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime UploadTimestamp { get; set; }

        #endregion
    }
}