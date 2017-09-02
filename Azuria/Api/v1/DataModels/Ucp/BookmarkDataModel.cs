using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    /// <summary>
    /// </summary>
    public class BookmarkDataModel : UcpEntryInfoDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int BookmarkId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("state")]
        public MediaStatus Status { get; set; }
    }
}