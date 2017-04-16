using Azuria.Api.v1.Converters;
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
        [JsonProperty("rate_flag", ItemConverterType = typeof(IntToBoolConverter))]
        public bool IsRated { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("spoiler_flag", ItemConverterType = typeof(IntToBoolConverter))]
        public bool IsSpoiler { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tid")]
        public int TagId { get; set; }

        #endregion
    }
}