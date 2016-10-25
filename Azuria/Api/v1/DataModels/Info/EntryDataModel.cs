using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
using Azuria.Media.Properties;
using Azuria.Search.Input;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class EntryDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("clicks")]
        internal int Clicks { get; set; }

        [JsonProperty("count")]
        internal int ContentCount { get; set; }

        [JsonProperty("description")]
        internal string Description { get; set; }

        [JsonProperty("id")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        [JsonProperty("kat")]
        [JsonConverter(typeof(CategoryConverter))]
        public MediaEntryType EntryType { get; set; }

        [JsonProperty("fsk")]
        [JsonConverter(typeof(FskConverter))]
        internal IEnumerable<FskType> Fsk { get; set; }

        [JsonProperty("genre")]
        [JsonConverter(typeof(GenreConverter))]
        internal IEnumerable<GenreType> Genre { get; set; }

        [JsonProperty("license")]
        [JsonConverter(typeof(IsLicensedConverter))]
        internal bool IsLicensed { get; set; }

        internal MediaRating Rating => new MediaRating(this.TotalStars, this.Voters);

        [JsonProperty("state")]
        internal MediaStatus Status { get; set; }

        [JsonProperty("rate_sum")]
        internal int TotalStars { get; set; }

        [JsonProperty("rate_count")]
        internal int Voters { get; set; }

        #endregion
    }
}