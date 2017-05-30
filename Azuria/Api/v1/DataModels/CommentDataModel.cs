using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Enums.User;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels
{
    /// <summary>
    /// </summary>
    public class CommentDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("comment")]
        public string CommentContent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int CommentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("episode")]
        public int ContentIndex { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tid")]
        public int EntryId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rating")]
        public int OverallRating { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("status")]
        public MediaProgressState State { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("data")]
        [JsonConverter(typeof(SubRatingsConverter))]
        public Dictionary<RatingCategory, int> SubRatings { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("positive")]
        public int Upvotes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uid")]
        public int UserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}