using Azuria.Api.Enums;
using Azuria.Api.Enums.Info;
using Azuria.Api.v1.Converters;
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
        [JsonProperty("fsk")]
        [JsonConverter(typeof(FskConverter))]
        public Fsk[] EntryFsk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("genre")]
        [JsonConverter(typeof(GenreConverter))]
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
        [JsonProperty("type")]
        [JsonConverter(typeof(IndustryTypeConverter))]
        public IndustryType IndustryType { get; set; }

        #endregion
    }
}