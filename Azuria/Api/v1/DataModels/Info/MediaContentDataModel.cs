using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class MediaContentDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("no")]
        public int ContentIndex { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("typ")]
        [JsonConverter(typeof(LanguageConverter))]
        public MediaLanguage Language { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("types")]
        [JsonConverter(typeof(StringCommaCollectionConverter))]
        public string[] StreamHosters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("typeimg")]
        [JsonConverter(typeof(StringCommaCollectionConverter))]
        public string[] StreamHosterImages { get; set; }

        /// <summary>
        /// Only available for chapters
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}