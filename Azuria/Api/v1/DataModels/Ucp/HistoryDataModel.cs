using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Ucp;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    internal class HistoryDataModel : IDataModel
    {
        #region Properties

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

        [JsonProperty("timestamp"), JsonConverter(typeof(CustomDateTimeConverter))]
        internal DateTime TimeStamp { get; set; }

        #endregion
    }
}