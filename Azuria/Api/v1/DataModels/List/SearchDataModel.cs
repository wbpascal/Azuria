using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// </summary>
    public class SearchDataModel : IEntryInfoDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("language", ItemConverterType = typeof(LanguageCommaCollectionConverter))]
        public MediaLanguage[] AvailableLanguages { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int ContentCount { get; set; }

        /// <inheritdoc />
        [JsonProperty("id")]
        public int EntryId { get; set; }

        /// <inheritdoc />
        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <inheritdoc />
        public MediaEntryType EntryType
            => (int) this.EntryMedium < 4 ? MediaEntryType.Anime : MediaEntryType.Manga;

        /// <summary>
        /// </summary>
        [JsonProperty("genre", ItemConverterType = typeof(GenreConverter))]
        public Genre[] Genre { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_count")]
        public int RateCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_sum")]
        public int RateSum { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("state")]
        public MediaStatus Status { get; set; }

        #endregion
    }
}