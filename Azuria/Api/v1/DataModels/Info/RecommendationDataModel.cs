using Azuria.Api.v1.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class RecommendationDataModel : EntryDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("count_positive")]
        public int CountPositive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("count_negative")]
        public int CountNegative { get; set; }

        /// <summary>
        /// Gets or sets whether the logged in user voted positive or negative for this recommendation. 
        /// If no user is logged in or the user did not vote for this recommendation yet, null is returned.
        /// </summary>
        [JsonProperty("positive")]
        [JsonConverter(typeof(IntToNullableBoolConverter))]
        public bool? VotedPositive { get; set; }
    }
}