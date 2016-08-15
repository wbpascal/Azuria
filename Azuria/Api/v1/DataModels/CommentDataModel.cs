using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.User;
using Azuria.User.Comment;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels
{
    internal class CommentDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("avatar")]
        internal string Avatar { get; set; }

        [JsonProperty("comment")]
        internal string CommentContent { get; set; }

        [JsonProperty("id")]
        internal int CommentId { get; set; }

        [JsonProperty("episode")]
        internal int ContentIndex { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime Date { get; set; }

        [JsonProperty("tid")]
        internal int EntryId { get; set; }

        [JsonProperty("rating")]
        internal int OverallRating { get; set; }

        [JsonProperty("status")]
        internal AnimeMangaProgressState State { get; set; }

        [JsonProperty("data"), JsonConverter(typeof(SubRatingsConverter))]
        internal Dictionary<RatingCategory, int> SubRatings { get; set; }

        [JsonProperty("positive")]
        internal int Upvotes { get; set; }

        [JsonProperty("uid")]
        internal int UserId { get; set; }

        [JsonProperty("username")]
        internal string Username { get; set; }

        #endregion
    }
}