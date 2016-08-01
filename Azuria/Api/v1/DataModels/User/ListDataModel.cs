using System;
using System.Collections.Generic;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
using Azuria.User;
using Azuria.User.Comment;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class ListDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("state")]
        internal AnimeMangaProgressState AuthorState { get; set; }

        [JsonProperty("comment")]
        internal string CommentContent { get; set; }

        [JsonProperty("episode")]
        internal int CommentContentIndex { get; set; }

        [JsonProperty("cid")]
        internal int CommentId { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime CommentLastChanged { get; set; }

        [JsonProperty("data"), JsonConverter(typeof(SubRatingsConverter))]
        internal Dictionary<RatingCategory, int> CommentSubRatings { get; set; }

        [JsonProperty("count")]
        internal int ContentCount { get; set; }

        [JsonProperty("id")]
        internal int EntryId { get; set; }

        [JsonProperty("name")]
        internal string EntryName { get; set; }

        [JsonProperty("estate")]
        internal AnimeMangaStatus EntryState { get; set; }

        [JsonProperty("medium"), JsonConverter(typeof(MediumConverter))]
        internal AnimeMangaMedium Medium { get; set; }

        [JsonProperty("rating")]
        internal int Rating { get; set; }

        #endregion
    }
}