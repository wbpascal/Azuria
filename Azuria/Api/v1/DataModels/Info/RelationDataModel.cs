using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class RelationDataModel : EntryDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageCommaCollectionConverter))]
        public MediaLanguage[] AvailableLanguages { get; set; }
    }
}