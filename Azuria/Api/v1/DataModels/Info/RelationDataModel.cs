using Azuria.Api.v1.Converter;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class RelationDataModel : EntryDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("language")]
        [JsonConverter(typeof(MediaLanguageCommaCollectionConverter))]
        public MediaLanguage[] AvailableLanguages { get; set; }

        /// <summary>
        /// Does not include the id of the season
        /// </summary>
        public SeasonDataModel StartSeason => new SeasonDataModel
        {
            EntryId = this.EntryId,
            Id = int.MinValue,
            Season = this.Season,
            Year = this.Year
        };

        [JsonProperty("season")] internal Season Season { get; set; }

        [JsonProperty("year")] internal int Year { get; set; }
    }
}