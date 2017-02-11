using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    internal class HistoryEnumerator<T> : PagedEnumerator<HistoryObject<T>> where T : IMediaObject
    {
        private const int ResultsPerPage = 50;
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;

        internal HistoryEnumerator(Senpai senpai, UserControlPanel controlPanel) : base(ResultsPerPage)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region Methods

        private static IMediaContent GetMediaContent(HistoryDataModel dataModel)
        {
            if (typeof(T) == typeof(Anime) ||
                typeof(T) == typeof(IMediaObject) && dataModel.EntryType == MediaEntryType.Anime)
                return new Episode(dataModel);
            if (typeof(T) == typeof(Manga) ||
                typeof(T) == typeof(IMediaObject) && dataModel.EntryType == MediaEntryType.Manga)
                return new Chapter(dataModel);

            return null;
        }

        /// <inheritdoc />
        protected override async Task<IProxerResult<IEnumerable<HistoryObject<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<HistoryDataModel[]> lResult = await RequestHandler.ApiRequest(
                    UcpRequestBuilder.GetHistory(nextPage, ResultsPerPage, this._senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<HistoryObject<T>>>(lResult.Exceptions);
            HistoryDataModel[] lData = lResult.Result;

            return new ProxerResult<IEnumerable<HistoryObject<T>>>(from historyDataModel in lData
                select new HistoryObject<T>(GetMediaContent(historyDataModel) as IMediaContent<T>,
                    historyDataModel.TimeStamp, this._controlPanel));
        }

        #endregion
    }
}