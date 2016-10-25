using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Azuria.Media.Properties;
using Azuria.Search.Input;
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
        public MediaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        [JsonProperty("kat")]
        public MediaEntryType EntryType { get; set; }

        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageConverter))]
        internal MediaLanguage Language { get; set; }

        [JsonProperty("state")]
        internal MediaStatus Status { get; set; }

        #endregion
    }
}