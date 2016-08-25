using System.Collections.Generic;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Search
{
    internal class SearchDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("language"), JsonConverter(typeof(LanguageCommaCollectionConverter))]
        internal AnimeMangaLanguage[] AvailableLanguages { get; set; }

        [JsonProperty("count")]
        internal int ContentCount { get; set; }

        [JsonProperty("id")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public AnimeMangaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        public AnimeMangaEntryType EntryType
            => (int) this.EntryMedium < 4 ? AnimeMangaEntryType.Anime : AnimeMangaEntryType.Manga;

        [JsonProperty("genre"), JsonConverter(typeof(GenreConverter))]
        internal IEnumerable<GenreType> Genre { get; set; }

        [JsonProperty("rate_count")]
        internal int RateCount { get; set; }

        [JsonProperty("rate_sum")]
        internal int RateSum { get; set; }

        internal AnimeMangaRating Rating => new AnimeMangaRating(this.RateSum, this.RateCount);

        [JsonProperty("state")]
        internal AnimeMangaStatus Status { get; set; }

        #endregion
    }
}