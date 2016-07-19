using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels
{
    /// <summary>
    /// </summary>
    public class LoginDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uid")]
        public int UserId { get; set; }

        #endregion
    }
}