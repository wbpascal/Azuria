using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    /// <summary>
    /// </summary>
    public class VoteDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("comment")]
        public string CommentContent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("kid")]
        public int CommentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

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
        [JsonProperty("id")]
        public int VoteId { get; set; }
    }
}