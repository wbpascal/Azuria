using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    internal class BookmarkDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("id")]
        internal int BookmarkId { get; set; }

        [JsonProperty("episode")]
        internal int ContentIndex { get; set; }

        [JsonProperty("eid")]
        internal int EntryId { get; set; }

        [JsonProperty("kat"), JsonConverter(typeof(CategoryConverter))]
        internal AnimeMangaEntryType EntryType { get; set; }

        [JsonProperty("language"), JsonConverter(typeof(LanguageConverter))]
        internal AnimeMangaLanguage Language { get; set; }

        [JsonProperty("medium"), JsonConverter(typeof(MediumConverter))]
        internal AnimeMangaMedium Medium { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("state")]
        internal AnimeMangaStatus Status { get; set; }

        #endregion
    }
}