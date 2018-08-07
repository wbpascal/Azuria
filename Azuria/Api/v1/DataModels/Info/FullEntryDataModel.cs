using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class FullEntryDataModel : EntryDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("lang")]
        [JsonConverter(typeof(MediaLanguageCollectionConverter))]
        public MediaLanguage[] AvailableLanguages { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("publisher")]
        public IndustryBasicDataModel[] Industry { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("gate")]
        public bool IsHContent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("names")]
        public EntryNameDataModel[] Names { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("seasons")]
        public SeasonDataModel[] Seasons { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tags")]
        public TagDataModel[] Tags { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("groups")]
        public TranslatorBasicDataModel[] Translator { get; set; }
    }
}