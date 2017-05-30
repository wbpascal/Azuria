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

        /// <inheritdoc />
        public bool Equals(SeasonDataModel other)
        {
            return this.Id == other.Id && this.Season == other.Season && this.Year == other.Year;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Id;
                hashCode = hashCode * 397 ^ (int) this.Season;
                hashCode = hashCode * 397 ^ this.Year;
                return hashCode;
            }
        }
    }
}