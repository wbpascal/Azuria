using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Anime;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Anime
{
    /// <summary>
    /// </summary>
    public class StreamDataModel
    {
        #region Properties

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
        [JsonConverter(typeof(StreamPartnerConverter))]
        public StreamHoster StreamHoster { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int StreamId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tid")]
        public int? TranslatorId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tname")]
        public string TranslatorName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uploader")]
        public int UploaderId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string UploaderName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime UploadTimestamp { get; set; }

        #endregion
    }
}