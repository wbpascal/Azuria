using Newtonsoft.Json;

namespace Azuria.Community
{
    internal class MessageModel
    {
        #region Properties

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("fromid")]
        public int Fromid { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        #endregion
    }

    internal class MessagesModel
    {
        #region Properties

        [JsonProperty("block")]
        public int Blocked { get; set; }

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("favour")]
        public int Favourite { get; set; }

        [JsonProperty("messages")]
        public MessageModel[] MessageModels { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        #endregion
    }
}