using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class SeasonDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("eid")]
        public int EntryId { get; set; } = int.MinValue;

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; } = int.MinValue;

        /// <summary>
        /// </summary>
        [JsonProperty("season")]
        public Season Season { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("year")]
        public int Year { get; set; }
    }
}