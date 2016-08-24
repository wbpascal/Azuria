using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    internal class VoteDataModel
    {
        #region Properties

        [JsonProperty("comment")]
        internal string CommentContent { get; set; }

        [JsonProperty("kid")]
        internal int CommentId { get; set; }

        [JsonProperty("name")]
        internal string EntryName { get; set; }

        [JsonProperty("rating")]
        internal int Rating { get; set; }

        [JsonProperty("type")]
        internal string Type { get; set; }

        [JsonProperty("uid")]
        internal int UserId { get; set; }

        [JsonProperty("username")]
        internal string Username { get; set; }

        [JsonProperty("id")]
        internal int VoteId { get; set; }

        #endregion
    }
}