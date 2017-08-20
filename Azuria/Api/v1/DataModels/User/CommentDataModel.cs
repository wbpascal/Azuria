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
        /// <inheritdoc />
        [JsonProperty("medium")]
        [JsonConverter(typeof(MediumConverter))]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("kat")]
        [JsonConverter(typeof(CategoryConverter))]
        public MediaEntryType EntryType { get; set; }
    }
}