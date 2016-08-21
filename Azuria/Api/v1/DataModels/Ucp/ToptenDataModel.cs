using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    internal class ToptenDataModel : User.ToptenDataModel
    {
        #region Properties

        [JsonProperty("fid")]
        internal int ToptenId { get; set; }

        #endregion
    }
}