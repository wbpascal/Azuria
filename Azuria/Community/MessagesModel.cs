using Newtonsoft.Json;

namespace Azuria.Community
{
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