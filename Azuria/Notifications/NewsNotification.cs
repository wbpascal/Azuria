using System;
using Azuria.Utilities;
using Newtonsoft.Json;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a news notification.
    /// </summary>
    public class NewsNotification : INotification
    {
        internal NewsNotification()
        {
            this.Type = NotificationType.News;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the message of the notification.
        /// </summary>
        public string Message => this.Description;

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the id of the author.
        /// </summary>
        [JsonProperty("uid")]
        public int AuthorId { get; set; }

        /// <summary>
        ///     Gets the username of the author.
        /// </summary>
        [JsonProperty("uname")]
        public string AuthorName { get; set; }

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

        /// <summary>
        ///     Obsolete. Gets the thread id.
        /// </summary>
        [JsonProperty("mid"), Obsolete("Use " + nameof(ThreadId))]
        internal int Mid { get; set; }

        /// <summary>
        ///     Gets the news id.
        /// </summary>
        [JsonProperty("nid")]
        public int NewsId { get; set; }

        /// <summary>
        ///     Obsolete.
        /// </summary>
        [JsonProperty("pid"), Obsolete]
        internal int Pid { get; set; }

        /// <summary>
        ///     Gets the post count of the news.
        /// </summary>
        [JsonProperty("posts")]
        public int Posts { get; set; }

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

        /// <summary>
        ///     Gets the date of the news as a unix timestamp.
        /// </summary>
        [JsonProperty("time")]
        public long Time { get; set; }

        #endregion

        #region

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Subject + "\n" + Utility.UnixTimeStampToDateTime(this.Time) + "\n" + this.CategoryName;
        }

        #endregion
    }
}