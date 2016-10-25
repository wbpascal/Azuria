using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Azuria.Media.Properties;
using Azuria.Search.Input;
using Azuria.UserInfo;
using Azuria.UserInfo.Comment;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class ListDataModel : IEntryInfoDataModel
    {
        #region Properties

        [JsonProperty("state")]
        internal MediaProgressState AuthorState { get; set; }

        [JsonProperty("comment")]
        internal string CommentContent { get; set; }

        [JsonProperty("episode")]
        internal int CommentContentIndex { get; set; }

        [JsonProperty("cid")]
        internal int CommentId { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime CommentLastChanged { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof(SubRatingsConverter))]
        internal Dictionary<RatingCategory, int> CommentSubRatings { get; set; }

        [JsonProperty("count")]
        internal int ContentCount { get; set; }

        [JsonProperty("id")]
        public int EntryId { get; set; }

        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        [JsonProperty("name")]
        public string EntryName { get; set; }

        [JsonProperty("estate")]
        internal MediaStatus EntryStatus { get; set; }

        public MediaEntryType EntryType
            => (int) this.EntryMedium < 4 ? MediaEntryType.Anime : MediaEntryType.Manga;

        [JsonProperty("rating")]
        internal int Rating { get; set; }

        #endregion
    }
}