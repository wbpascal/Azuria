using Newtonsoft.Json;

namespace Proxer.API.Community.ConferenceHelper
{
    internal class MessageModel
    {
        #region Properties

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("fromid")]
        public string Fromid { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        #endregion
    }

    internal class MessagesViewModel
    {
        #region Properties

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("messages")]
        public MessageModel[] MessagesModel { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        #endregion
    }
}