using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class AdaptionDataModel : DataModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int EntryId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("medium")]
        [JsonConverter(typeof(MediumConverter))]
        public MediaMedium Medium { get; set; }
    }
}