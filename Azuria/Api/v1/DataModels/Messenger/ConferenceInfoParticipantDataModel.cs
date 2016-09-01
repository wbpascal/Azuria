using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    internal class ConferenceInfoParticipantDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("avatar")]
        internal string AvatarId { get; set; }

        [JsonProperty("uid")]
        internal int UserId { get; set; }

        [JsonProperty("username")]
        internal string Username { get; set; }

        [JsonProperty("status")]
        internal string UserStatus { get; set; }

        #endregion
    }
}