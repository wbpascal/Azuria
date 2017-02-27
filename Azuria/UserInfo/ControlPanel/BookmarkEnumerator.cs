using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    internal class BookmarkEnumerator<T> : PagedEnumerator<Bookmark<T>> where T : IMediaObject
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

        /// <inheritdoc />
        protected override async Task<IProxerResult<IEnumerable<Bookmark<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<BookmarkDataModel[]> lResult = await RequestHandler.ApiRequest(
                    UcpRequestBuilder.GetReminder(this._senpai, GetCategory(), nextPage, ResultsPerPage))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<Bookmark<T>>>(lResult.Exceptions);
            BookmarkDataModel[] lData = lResult.Result;

            return new ProxerResult<IEnumerable<Bookmark<T>>>((from bookmarkDataModel in lData
                select GetBookmark(bookmarkDataModel)).Where(bookmark => bookmark != null));

            string GetCategory()
            {
                if (typeof(T) == typeof(Anime) || typeof(T) == typeof(Manga))
                    return typeof(T).GetTypeInfo().Name.ToLowerInvariant();
                return string.Empty;
            }

            Bookmark<T> GetBookmark(BookmarkDataModel dataModel)
            {
                if (typeof(T) != typeof(Manga) && dataModel.EntryType == MediaEntryType.Anime)
                    return new Bookmark<T>((IMediaContent<T>) new Episode(dataModel), dataModel.BookmarkId,
                        this._controlPanel);
                if (typeof(T) != typeof(Anime) && dataModel.EntryType == MediaEntryType.Manga)
                    return new Bookmark<T>((IMediaContent<T>) new Chapter(dataModel), dataModel.BookmarkId,
                        this._controlPanel);

                return null;
            }
        }

        #endregion
    }
}