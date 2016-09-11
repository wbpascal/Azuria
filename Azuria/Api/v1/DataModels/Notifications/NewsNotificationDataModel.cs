using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Notifications
{
    internal class NewsNotificationDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("uid")]
        internal int AuthorId { get; set; }

        [JsonProperty("uname")]
        internal string AuthorName { get; set; }

        [JsonProperty("catid")]
        internal int CategoryId { get; set; }

        [JsonProperty("catname")]
        internal string CategoryName { get; set; }

        [JsonProperty("description")]
        internal string Description { get; set; }

        [JsonProperty("hits")]
        internal int Hits { get; set; }

        [JsonProperty("image_id")]
        internal string ImageId { get; set; }

        [JsonProperty("image_style")]
        internal string ImageStyle { get; set; }

        [JsonProperty("nid")]
        internal int NewsId { get; set; }

        [JsonProperty("posts")]
        internal int Posts { get; set; }

        [JsonProperty("subject")]
        internal string Subject { get; set; }

        [JsonProperty("thread")]
        internal int ThreadId { get; set; }

        [JsonProperty("time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime TimeStamp { get; set; }

        #endregion
    }
}