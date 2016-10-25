using Azuria.Api.v1.Enums;
using Azuria.Search.Input;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class ToptenDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("eid")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        [JsonProperty("kat")]
        public MediaEntryType EntryType { get; set; }

        #endregion
    }
}