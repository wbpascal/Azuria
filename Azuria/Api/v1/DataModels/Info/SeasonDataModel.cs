using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class SeasonDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

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