using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    /// <summary>
    /// </summary>
    public class ToptenDataModel : User.ToptenDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("fid")]
        public int ToptenId { get; set; }

        #endregion
    }
}