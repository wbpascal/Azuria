using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    internal class ConstantsDataModel : IDataModel
    {
        [JsonProperty("textCount")]
        internal int MaxCharactersPerMessage { get; set; }

        [JsonProperty("conferenceLimit")]
        internal int ConferencesPerPage { get; set; }

        [JsonProperty("messagesLimit")]
        internal int MessagesPerPage { get; set; }

        [JsonProperty("userLimit")]
        internal int MaxUsersPerConference { get; set; }

        [JsonProperty("topicCount")]
        internal int MaxCharactersTopic { get; set; }
    }
}
