using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    internal class HistoryDataModel : IEntryInfoDataModel
    {
        #region Properties

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

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        internal DateTime TimeStamp { get; set; }

        #endregion
    }
}