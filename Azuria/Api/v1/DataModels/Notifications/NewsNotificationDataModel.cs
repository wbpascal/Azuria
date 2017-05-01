using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Notifications
{
    /// <summary>
    /// </summary>
    public class NewsNotificationDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("uid")]
        public int AuthorId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uname")]
        public string AuthorName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("catid")]
        public int CategoryId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("catname")]
        public string CategoryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("hits")]
        public int Hits { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("image_style")]
        public string ImageStyle { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("nid")]
        public int NewsId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("posts")]
        public int Posts { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("thread")]
        public int ThreadId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime TimeStamp { get; set; }

        #endregion
    }
}