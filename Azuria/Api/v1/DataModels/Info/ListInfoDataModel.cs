using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class ListInfoDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("kat")]
        [JsonConverter(typeof(CategoryConverter))]
        internal MediaEntryType Category { get; set; }

        [JsonProperty("episodes")]
        internal MediaContentDataModel[] ContentObjects { get; set; }

        [JsonProperty("end")]
        internal int EndIndex { get; set; }

        [JsonProperty("start")]
        internal int StartIndex { get; set; }

        #endregion
    }
}