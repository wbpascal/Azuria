using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    internal class BookmarkDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("id")]
        internal int BookmarkId { get; set; }

        [JsonProperty("episode")]
        internal int ContentIndex { get; set; }

        [JsonProperty("eid")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public AnimeMangaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        [JsonProperty("kat")]
        public AnimeMangaEntryType EntryType { get; set; }

        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageConverter))]
        internal AnimeMangaLanguage Language { get; set; }

        [JsonProperty("state")]
        internal AnimeMangaStatus Status { get; set; }

        #endregion
    }
}