using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converter;
using Azuria.Enums.User;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class CommentDataModel : DataModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Uri Avatar => new Uri(ApiConstants.ProxerAvatarShortCdnUrl + this.AvatarId);

        [JsonProperty("avatar")] internal string AvatarId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("comment")]
        public string CommentText { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int CommentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("episode")]
        public int Progress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tid")]
        public int EntryId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("state")]
        public MediaProgressState State { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("data")]
        [JsonConverter(typeof(SubRatingsConverter))]
        public IReadOnlyDictionary<RatingCategory, int> SubRatings { get; set; } =
            new Dictionary<RatingCategory, int>();

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

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