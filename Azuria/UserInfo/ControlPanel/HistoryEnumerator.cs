using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.UserInfo.ControlPanel
{
    internal class HistoryEnumerator<T> : PageEnumerator<HistoryObject<T>> where T : IAnimeMangaObject
    {
        private const int ResultsPerPage = 50;
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;

        internal HistoryEnumerator(Senpai senpai, UserControlPanel controlPanel)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region

        private static IAnimeMangaContent<T> GetAnimeMangaContent(HistoryDataModel dataModel)
        {
            if ((typeof(T) == typeof(Anime)) ||
                ((typeof(T) == typeof(IAnimeMangaObject)) && (dataModel.EntryType == AnimeMangaEntryType.Anime)))
                return (IAnimeMangaContent<T>) new Anime.Episode(dataModel);
            if ((typeof(T) == typeof(Manga)) ||
                ((typeof(T) == typeof(IAnimeMangaObject)) && (dataModel.EntryType == AnimeMangaEntryType.Manga)))
                return (IAnimeMangaContent<T>) new Manga.Chapter(dataModel);

            return null;
        }

        internal override async Task<ProxerResult<IEnumerable<HistoryObject<T>>>> GetNextPage(int nextPage)
        {
            ProxerResult<ProxerApiResponse<HistoryDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetHistory(nextPage, ResultsPerPage,
                        this._senpai));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<HistoryObject<T>>>(lResult.Exceptions);
            HistoryDataModel[] lData = lResult.Result.Data;

            return new ProxerResult<IEnumerable<HistoryObject<T>>>(from historyDataModel in lData
                select
                new HistoryObject<T>(GetAnimeMangaContent(historyDataModel), historyDataModel.TimeStamp,
                    this._controlPanel));
        }

        #endregion
    }
}