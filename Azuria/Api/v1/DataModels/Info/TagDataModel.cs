using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class TagDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_flag")]
        [JsonConverter(typeof(IntToBoolConverter))]
        public bool IsRated { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("spoiler_flag")]
        [JsonConverter(typeof(IntToBoolConverter))]
        public bool IsSpoiler { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tag")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tid")]
        public int TagId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}