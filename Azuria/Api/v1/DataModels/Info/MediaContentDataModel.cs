using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class MediaContentDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("no", Required = Required.Always)]
        public int ContentIndex { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("typ", Required = Required.Always)]
        [JsonConverter(typeof(LanguageConverter))]
        public MediaLanguage Language { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("types")]
        [JsonConverter(typeof(StreamHosterConverter))]
        public StreamHoster[] StreamHosters { get; set; }

        /// <summary>
        /// Only available for chapters
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion
    }
}