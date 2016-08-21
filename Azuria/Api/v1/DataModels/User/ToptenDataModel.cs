using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class ToptenDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("eid")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public AnimeMangaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        [JsonProperty("kat")]
        public AnimeMangaEntryType EntryType { get; set; }

        #endregion
    }
}