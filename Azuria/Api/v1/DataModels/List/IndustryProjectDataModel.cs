using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public class IndustryProjectDataModel : IEntryInfoDataModel
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("fsk", ItemConverterType = typeof(FskConverter))]
        public Fsk[] EntryFsk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("genre", ItemConverterType = typeof(GenreConverter))]
        public Genre[] EntryGenre { get; set; }

        /// <inheritdoc />
        [JsonProperty("id")]
        public int EntryId { get; set; }

        /// <inheritdoc />
        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_count")]
        public int EntryRatingsCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_sum")]
        public int EntryRatingsSum { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("state")]
        public MediaStatus EntryStatus { get; set; }

        /// <inheritdoc />
        public MediaEntryType EntryType => (int) this.EntryMedium < 4 ? MediaEntryType.Anime : MediaEntryType.Manga;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type", ItemConverterType = typeof(IndustryTypeConverter))]
        public IndustryType IndustryType { get; set; }

        #endregion
    }
}