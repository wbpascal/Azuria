using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class GroupDataModel : IDataModel
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