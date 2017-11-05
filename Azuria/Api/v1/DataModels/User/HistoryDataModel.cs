using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    /// <summary>
    /// </summary>
    public class HistoryDataModel : DataModelBase, IEntryInfoDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("episode")]
        public int ContentIndex { get; set; }

        /// <inheritdoc />
        [JsonProperty("eid")]
        public int EntryId { get; set; }

        /// <inheritdoc />
        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <inheritdoc />
        [JsonProperty("kat")]
        public MediaEntryType EntryType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("language", ItemConverterType = typeof(LanguageConverter))]
        public MediaLanguage Language { get; set; }
    }
}