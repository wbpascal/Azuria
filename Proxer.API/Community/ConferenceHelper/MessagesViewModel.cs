using System.Collections.Generic;
using Newtonsoft.Json;

namespace Proxer.API.Community.ConferenceHelper
{

    internal class MessageModel
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fromid")]
        public string Fromid { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("block")]
        public string Block { get; set; }
    }

    internal class MessagesViewModel
    {

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("messages")]
        public MessageModel[] MessagesModel { get; set; }

        [JsonProperty("favour")]
        public string Favour { get; set; }

        [JsonProperty("block")]
        public string Block { get; set; }

        [JsonProperty("bookmark")]
        public string Bookmark { get; set; }
    }

}