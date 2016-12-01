using Azuria.Api.v1.Converters;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class MediaTagDataModel : IDataModel
    {
        #region Properties

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
        [JsonProperty("tid")]
        public TagType Tag { get; set; }

        #endregion
    }
}