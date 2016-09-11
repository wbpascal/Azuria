using System;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.UserInfo;

namespace Azuria.Notifications.News
{
    /// <summary>
    ///     Represents a news notification.
    /// </summary>
    public class NewsNotification : INotification
    {
        private const string ProxerCdn = "https://cdn.proxer.me";
        private readonly NewsNotificationDataModel _dataModel;

        internal NewsNotification(NewsNotificationDataModel dataModel)
        {
            this._dataModel = dataModel;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public User Author => new User(this._dataModel.AuthorName, this._dataModel.AuthorId);

        /// <summary>
        ///     Gets the category id of the news.
        /// </summary>
        public int CategoryId => this._dataModel.CategoryId;

        /// <summary>
        ///     Gets the category name of the news.
        /// </summary>
        public string CategoryName => this._dataModel.CategoryName;

        /// <summary>
        ///     Gets the description of the news.
        /// </summary>
        public string Description => this._dataModel.Description;

        /// <summary>
        ///     Gets the hits of the news.
        /// </summary>
        public int Hits => this._dataModel.Hits;

        /// <summary>
        ///     Gets the title image of the news.
        /// </summary>
        public Uri Image => new Uri($"{ProxerCdn}/news/{this._dataModel.NewsId}_{this._dataModel.ImageId}.png");

        /// <summary>
        ///     Gets infos about the css style of the image.
        /// </summary>
        public string ImageStyle => this._dataModel.ImageStyle;

        /// <summary>
        ///     Gets the title image of the news.
        /// </summary>
        public Uri ImageThumbnail
            => new Uri($"{ProxerCdn}/th/{this._dataModel.NewsId}_{this._dataModel.ImageId}.png");

        /// <summary>
        ///     Gets the news id.
        /// </summary>
        public int NewsId => this._dataModel.NewsId;

        /// <summary>
        ///     Gets the id of the notification.
        /// </summary>
        public string NotificationId
            => $"{this.Author.Id}_{this.CategoryId}_{this.ThreadId}_{this.TimeStamp.ToFileTime()}";

        /// <summary>
        ///     Gets the post count of the news.
        /// </summary>
        public int Posts => this._dataModel.Posts;

        /// <summary>
        ///     Gets the headline of the news.
        /// </summary>
        public string Subject => this._dataModel.Subject;

        /// <summary>
        ///     Gets the thread id.
        /// </summary>
        public int ThreadId => this._dataModel.ThreadId;

        /// <summary>
        /// </summary>
        public DateTime TimeStamp => this._dataModel.TimeStamp;

        #endregion
    }
}