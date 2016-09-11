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
    public sealed class NewsNotificationEnumerator : PageEnumerator<NewsNotification>
    {
        private const int NewsPerPage = 15;
        private readonly Senpai _senpai;

        internal NewsNotificationEnumerator(Senpai senpai) : base(NewsPerPage)
        {
            this._senpai = senpai;
        }

        #region Methods

        #region Overrides of PageEnumerator<NewsNotification>

        internal override async Task<ProxerResult<IEnumerable<NewsNotification>>> GetNextPage(int nextPage)
        {
            ProxerResult<ProxerApiResponse<NewsNotificationDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.NotificationGetNews(nextPage, NewsPerPage, this._senpai));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<NewsNotification>>(lResult.Exceptions);

            return
                new ProxerResult<IEnumerable<NewsNotification>>(from newsNotificationDataModel in lResult.Result.Data
                    select new NewsNotification(newsNotificationDataModel));
        }

        #endregion

        #endregion
    }
}