using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    internal class BookmarkEnumerator<T> : PagedEnumerator<Bookmark<T>> where T : class, IMediaObject
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

        internal override async Task<IProxerResult<IEnumerable<Bookmark<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<BookmarkDataModel[]> lResult = await RequestHandler.ApiRequest(
                    UcpRequestBuilder.GetReminder(typeof(T).GetTypeInfo().Name.ToLowerInvariant(),
                        nextPage, ResultsPerPage, this._senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<Bookmark<T>>>(lResult.Exceptions);
            BookmarkDataModel[] lData = lResult.Result;

            return new ProxerResult<IEnumerable<Bookmark<T>>>(from bookmarkDataModel in lData
                select new Bookmark<T>(
                    typeof(T) == typeof(Anime)
                        ? (IMediaContent<T>) new Episode(bookmarkDataModel)
                        : (IMediaContent<T>) new Chapter(bookmarkDataModel),
                    bookmarkDataModel.BookmarkId, this._controlPanel));
        }

        #endregion
    }
}