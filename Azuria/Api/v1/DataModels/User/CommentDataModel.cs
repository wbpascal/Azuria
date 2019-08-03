using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentDataModel : Info.CommentDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("medium")]
        [JsonConverter(typeof(MediumConverter))]
        public MediaMedium EntryMedium { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("kat")]
        [JsonConverter(typeof(CategoryConverter))]
        public MediaEntryType EntryType { get; set; }
    }
}