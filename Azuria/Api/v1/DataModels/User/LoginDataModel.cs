using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class LoginDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("avatar")]
        internal string Avatar { get; set; }

        [JsonProperty("token")]
        internal string Token { get; set; }

        [JsonProperty("uid")]
        internal int UserId { get; set; }

        #endregion
    }
}