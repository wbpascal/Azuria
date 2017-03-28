using Azuria.Api.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    /// <summary>
    /// </summary>
    public class ToptenDataModel : IEntryInfoDataModel
    {
        #region Properties

        /// <inheritdoc />
        [JsonProperty("eid")]
        public int EntryId { get; set; }

        /// <inheritdoc />
        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <inheritdoc />
        [JsonProperty("kat")]
        public MediaEntryType EntryType { get; set; }

        #endregion
    }
}