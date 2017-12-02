using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class ConstantsDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("conferenceLimit")]
        public int ConferencesPerPage { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("textCount")]
        public int MaxCharactersPerMessage { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("topicCount")]
        public int MaxCharactersTopic { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("userLimit")]
        public int MaxUsersPerConference { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("messagesLimit")]
        public int MessagesPerPage { get; set; }
    }
}