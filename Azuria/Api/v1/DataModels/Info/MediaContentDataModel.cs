using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class MediaContentDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("no", Required = Required.Always)]
        internal int ContentIndex { get; set; }

        [JsonProperty("typ", Required = Required.Always)]
        [JsonConverter(typeof(LanguageConverter))]
        internal MediaLanguage Language { get; set; }

        [JsonProperty("types")]
        [JsonConverter(typeof(StreamHosterConverter))]
        internal IEnumerable<StreamHoster> StreamHosters { get; set; }

        /// <summary>
        /// Only available for chapters
        /// </summary>
        [JsonProperty("title")]
        internal string Title { get; set; }

        #endregion
    }
}