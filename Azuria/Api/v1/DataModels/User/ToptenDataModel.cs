using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class ToptenDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("eid")]
        internal int EntryId { get; set; }

        [JsonProperty("kat")]
        internal AnimeMangaEntryType EntryType { get; set; }

        [JsonProperty("medium")]
        internal AnimeMangaMedium Medium { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        #endregion
    }
}