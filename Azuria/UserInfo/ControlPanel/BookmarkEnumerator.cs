using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Utilities;

namespace Azuria.UserInfo.ControlPanel
{
    internal class BookmarkEnumerator<T> : PageEnumerator<BookmarkObject<T>> where T : class, IMediaObject
    {
        private const int ResultsPerPage = 100;
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;

        internal BookmarkEnumerator(Senpai senpai, UserControlPanel controlPanel) : base(ResultsPerPage)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region Methods

        internal override async Task<ProxerResult<IEnumerable<BookmarkObject<T>>>> GetNextPage(int nextPage)
        {
            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(
                        ApiRequestBuilder.UcpGetReminder(typeof(T).GetTypeInfo().Name.ToLowerInvariant(),
                            nextPage, ResultsPerPage, this._senpai));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<BookmarkObject<T>>>(lResult.Exceptions);
            BookmarkDataModel[] lData = lResult.Result.Data;

            return new ProxerResult<IEnumerable<BookmarkObject<T>>>(from bookmarkDataModel in lData
                select new BookmarkObject<T>(
                    typeof(T) == typeof(Anime)
                        ? (IMediaContent<T>) new Anime.Episode(bookmarkDataModel)
                        : (IMediaContent<T>) new Manga.Chapter(bookmarkDataModel),
                    bookmarkDataModel.BookmarkId, this._controlPanel));
        }

        #endregion
    }
}