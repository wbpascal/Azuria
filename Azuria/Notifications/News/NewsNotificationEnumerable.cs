using Azuria.Enumerable;

namespace Azuria.Notifications.News
{
    /// <summary>
    /// Represents a collection of news notifications.
    /// </summary>
    public class NewsNotificationEnumerable : PagedEnumerable<NewsNotification>
    {
        private readonly int _newsPerPage;
        private readonly Senpai _senpai;

        internal NewsNotificationEnumerable(Senpai senpai, int newsPerPage = 15)
        {
            this._senpai = senpai;
            this._newsPerPage = newsPerPage;
        }

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public override PagedEnumerator<NewsNotification> GetEnumerator()
        {
            return new NewsNotificationEnumerator(this._senpai, this._newsPerPage);
        }

        #endregion
    }
}