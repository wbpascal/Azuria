using System;
using Azuria.Api.Enums;
using Azuria.Api.Enums.Info;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Ucp;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    /// <summary>
    /// </summary>
    public class HistoryDataModel : IEntryInfoDataModel
    {
        #region Properties

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
        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageConverter))]
        public MediaLanguage Language { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TimeStamp { get; set; }

        #endregion
    }
}