using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class TranslatorDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("country")]
        [JsonConverter(typeof(CountryConverter))]
        public Language Language { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}