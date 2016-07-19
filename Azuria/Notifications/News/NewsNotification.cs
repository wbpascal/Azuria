using System;
using Azuria.Utilities;
using Newtonsoft.Json;

namespace Azuria.Notifications.News
{
    /// <summary>
    ///     Represents a news notification.
    /// </summary>
    public class NewsNotification : INotification
    {
        internal NewsNotification()
        {
        }

        #region Properties

        /// <summary>
        /// </summary>
        public User.User Author => new User.User(this.AuthorName, this.AuthorId, this.Senpai);

        [JsonProperty("uid")]
        internal int AuthorId { get; set; }

        [JsonProperty("uname")]
        internal string AuthorName { get; set; }

        /// <summary>
        ///     Gets the category id of the news.
        /// </summary>
        [JsonProperty("catid")]
        public int CategoryId { get; set; }

        /// <summary>
        ///     Gets the category name of the news.
        /// </summary>
        [JsonProperty("catname")]
        public string CategoryName { get; set; }

        /// <summary>
        ///     Gets the description of the news.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets the hits of the news.
        /// </summary>
        [JsonProperty("hits")]
        public int Hits { get; set; }

        /// <summary>
        ///     Gets the title image of the news.
        /// </summary>
        public Uri Image => new Uri($"http://cdn.proxer.me/news/{this.NewsId}_{this.ImageId}.png");

        /// <summary>
        ///     Gets the image id with the help of which the image can be retrieved.
        /// </summary>
        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        /// <summary>
        ///     Gets infos about the css style of the image.
        /// </summary>
        /// <seealso cref="ImageId" />
        [JsonProperty("image_style")]
        public string ImageStyle { get; set; }

        [JsonProperty("mid"), Obsolete("Use " + nameof(ThreadId))]
        internal int Mid { get; set; }

        /// <summary>
        ///     Gets the news id.
        /// </summary>
        [JsonProperty("nid")]
        public int NewsId { get; set; }

        /// <summary>
        ///     Gets the id of the notification.
        /// </summary>
        public string NotificationId => this.AuthorId.ToString() + this.CategoryId + this.ThreadId + this.Time;

        [JsonProperty("pid"), Obsolete]
        internal int Pid { get; set; }

        /// <summary>
        ///     Gets the post count of the news.
        /// </summary>
        [JsonProperty("posts")]
        public int Posts { get; set; }

        internal Senpai Senpai { get; set; }

        /// <summary>
        ///     Gets the headline of the news.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        ///     Gets the thread id.
        /// </summary>
        [JsonProperty("thread")]
        public int ThreadId { get; set; }

        [JsonProperty("time")]
        internal long Time { get; set; }

        /// <summary>
        /// </summary>
        public DateTime TimeStamp => Utility.UnixTimeStampToDateTime(this.Time);

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => NotificationType.News;

        #endregion
    }
}