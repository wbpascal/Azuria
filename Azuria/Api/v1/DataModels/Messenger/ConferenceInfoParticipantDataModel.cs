using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class ConferenceInfoParticipantDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("avatar")]
        public string AvatarId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uid")]
        public int UserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("status")]
        public string UserStatus { get; set; }
    }
}