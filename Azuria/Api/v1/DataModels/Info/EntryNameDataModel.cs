using Azuria.Api.v1.Converter.Info;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class EntryNameDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("eid")]
        public int EntryId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(EntryNameTypeConverter))]
        public MediaNameType Type { get; set; }
    }
}