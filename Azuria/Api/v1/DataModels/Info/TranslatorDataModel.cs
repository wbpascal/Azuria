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

        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("country")]
        [JsonConverter(typeof(GroupLanguageConverter))]
        internal Language Language { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        #endregion
    }
}