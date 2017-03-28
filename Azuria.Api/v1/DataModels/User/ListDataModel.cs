using System;
using System.Collections.Generic;
using Azuria.Api.Enums;
using Azuria.Api.Enums.Info;
using Azuria.Api.v1.Converters;
using Azuria.UserInfo;
using Azuria.UserInfo.Comment;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    /// <summary>
    /// </summary>
    public class ListDataModel : IEntryInfoDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("comment")]
        public string CommentContent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("episode")]
        public int CommentContentIndex { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("cid")]
        public int CommentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime CommentLastChanged { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("state")]
        public MediaProgressState CommentState { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("data")]
        [JsonConverter(typeof(SubRatingsConverter))]
        public Dictionary<RatingCategory, int> CommentSubRatings { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int ContentCount { get; set; }

        /// <inheritdoc />
        [JsonProperty("id")]
        public int EntryId { get; set; }

        /// <inheritdoc />
        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("estate")]
        public MediaStatus EntryStatus { get; set; }

        /// <inheritdoc />
        public MediaEntryType EntryType
            => (int) this.EntryMedium < 4 ? MediaEntryType.Anime : MediaEntryType.Manga;

        /// <summary>
        /// </summary>
        [JsonProperty("rating")]
        public int Rating { get; set; }

        #endregion
    }
}