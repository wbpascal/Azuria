using System;
using Azuria.Api.v1.Converter;
using Azuria.Api.v1.DataModels.Media;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Anime
{
    /// <summary>
    /// </summary>
    public class StreamDataModel : UploadedMediaDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("entryname")]
        public string EntryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string HosterFullName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("img")]
        public string HosterImageFileName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("htype")]
        public string HostingType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("replace")]
        public string PlaceholderLink { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("type")]
        public string StreamHoster { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int StreamId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime UploadTimestamp { get; set; }
    }
}