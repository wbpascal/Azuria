using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProjectDataModel : DataModelBase, IEntryInfoDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("fsk")]
        [JsonConverter(typeof(FskConverter))]
        public Fsk[] EntryFsk { get; set; }

        /// <summary>
        /// </summary>
        public Genre[] EntryGenre => GenreConverter.ParseGenre(this.EntryGenreRaw);

        /// <summary>
        /// Raw value of <see cref="EntryGenre"/>
        /// </summary>
        [JsonProperty("genre")]
        public string EntryGenreRaw { get; set; }

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
    }
}