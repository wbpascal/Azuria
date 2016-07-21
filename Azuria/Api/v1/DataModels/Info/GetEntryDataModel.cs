using System.Collections.Generic;
using Azuria.AnimeManga;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class GetEntryDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("clicks")]
        internal int Clicks { get; set; }

        [JsonProperty("count")]
        internal int ContentCount { get; set; }

        [JsonProperty("description")]
        internal string Description { get; set; }

        [JsonProperty("kat")]
        [JsonConverter(typeof(CategoryConverter))]
        internal AnimeMangaEntryType EntryType { get; set; }

        [JsonProperty("fsk")]
        [JsonConverter(typeof(FskConverter))]
        internal IEnumerable<FskType> Fsk { get; set; }

        [JsonProperty("genre")]
        [JsonConverter(typeof(GenreConverter))]
        internal IEnumerable<GenreType> Genre { get; set; }

        [JsonProperty("medium")]
        [JsonConverter(typeof(AnimeMangaMediumConverter))]
        internal AnimeMangaMedium Medium { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        internal AnimeMangaRating Rating => new AnimeMangaRating(this.TotalStars, this.Voters);

        [JsonProperty("state")]
        internal AnimeMangaStatus State { get; set; }

        [JsonProperty("rate_sum")]
        internal int TotalStars { get; set; }

        [JsonProperty("rate_count")]
        internal int Voters { get; set; }

        #endregion
    }
}