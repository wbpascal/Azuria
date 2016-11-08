using Azuria.Api.v1.Converters.Info;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class FullEntryDataModel : EntryDataModel
    {
        #region Properties

        [JsonProperty("lang")]
        [JsonConverter(typeof(LanguageCollectionConverter))]
        internal MediaLanguage[] AvailableLanguages { get; set; }

        [JsonProperty("gate")]
        internal bool IsHContent { get; set; }

        [JsonProperty("names")]
        internal NameDataModel[] Names { get; set; }

        [JsonProperty("publisher")]
        internal PublisherDataModel[] Publisher { get; set; }

        [JsonProperty("seasons")]
        internal SeasonDataModel[] Seasons { get; set; }

        [JsonProperty("tags")]
        internal MediaTagDataModel[] Tags { get; set; }

        [JsonProperty("groups")]
        internal TranslatorDataModel[] Translator { get; set; }

        #endregion
    }
}