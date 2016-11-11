using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Azuria.Media.Properties;
using Azuria.Search.Input;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Search
{
    internal class SearchDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageCommaCollectionConverter))]
        internal IEnumerable<MediaLanguage> AvailableLanguages { get; set; }

        [JsonProperty("count")]
        internal int ContentCount { get; set; }

        [JsonProperty("id")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        public MediaEntryType EntryType
            => (int) this.EntryMedium < 4 ? MediaEntryType.Anime : MediaEntryType.Manga;

        [JsonProperty("genre")]
        [JsonConverter(typeof(GenreConverter))]
        internal IEnumerable<GenreType> Genre { get; set; }

        [JsonProperty("rate_count")]
        internal int RateCount { get; set; }

        [JsonProperty("rate_sum")]
        internal int RateSum { get; set; }

        internal MediaRating Rating => new MediaRating(this.RateSum, this.RateCount);

        [JsonProperty("state")]
        internal MediaStatus Status { get; set; }

        #endregion
    }
}