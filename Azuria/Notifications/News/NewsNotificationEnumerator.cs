using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.ErrorHandling;
using Azuria.Utilities;

namespace Azuria.Notifications.News
{
    /// <summary>
    /// </summary>
    internal sealed class NewsNotificationEnumerator : PageEnumerator<NewsNotification>
    {
        private readonly int _newsPerPage;
        private readonly Senpai _senpai;

        internal NewsNotificationEnumerator(Senpai senpai, int newsPerPage = 15) : base(newsPerPage)
        {
            this._newsPerPage = newsPerPage;
            this._senpai = senpai;
        }

        #region Methods

        internal override async Task<IProxerResult<IEnumerable<NewsNotification>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<NewsNotificationDataModel[]> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.NotificationGetNews(
                    nextPage, this._newsPerPage, this._senpai));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<NewsNotification>>(lResult.Exceptions);

            return
                new ProxerResult<IEnumerable<NewsNotification>>(from newsNotificationDataModel in lResult.Result
                    select new NewsNotification(newsNotificationDataModel, this._senpai));
        }

        #endregion
    }
}