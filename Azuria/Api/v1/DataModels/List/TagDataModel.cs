using Azuria.Api.v1.Converters.List;
using Azuria.Enums.List;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
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
        [JsonProperty("tag")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("subtype", ItemConverterType = typeof(TagSubtypeConverter))]
        public TagSubtype Subtype { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("type", ItemConverterType = typeof(TagTypeConverter))]
        public TagType TagType { get; set; }
    }
}