using Azuria.Api.v1.Converters.Info;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class PublisherDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("country")]
        [JsonConverter(typeof(PublisherCountryConverter))]
        internal Country Country { get; set; }

        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(PublisherTypeConverter))]
        internal IndustryType Type { get; set; }

        #endregion
    }
}