using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class FullEntryDataModel : EntryDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("lang")]
        [JsonConverter(typeof(LanguageCollectionConverter))]
        public MediaLanguage[] AvailableLanguages { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("gate")]
        public bool IsHContent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("names")]
        public NameDataModel[] Names { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("publisher")]
        public PublisherDataModel[] Publisher { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("seasons")]
        public SeasonDataModel[] Seasons { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tags")]
        public MediaTagDataModel[] Tags { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("groups")]
        public TranslatorDataModel[] Translator { get; set; }

        #endregion
    }
}