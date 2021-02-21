using Azuria.Api.v1.Converter.List;
using Azuria.Enums.List;
using Azuria.Helpers.Extensions;
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
        public TagSubtype? Subtype => TagSubtypeConverter.ParseSubtype(this.SubtypeRaw);

        /// <summary>
        /// </summary>
        [JsonProperty("subtype")]
        public string SubtypeRaw { get; set; }

        /// <summary>
        /// </summary>
        public TagType? TagType => TagTypeConverter.ParseTagType(this.SubtypeRaw);

        /// <summary>
        /// </summary>
        [JsonProperty("type")]
        public string TagTypeRaw { get; set; }
    }
}