using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Api.v1.Enums;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Search.Input;

namespace Azuria.Search
{
    internal class SearchResultEnumerator<T> : PagedEnumerator<T> where T : IMediaObject
    {
        private const int ResultsPerPage = 100;
        private readonly SearchInput _input;

        internal SearchResultEnumerator(SearchInput input) : base(ResultsPerPage)
        {
            this._input = input;
        }

        #region Methods

        private static IEnumerable<T> GetAnimeList(IEnumerable<SearchDataModel> dataModels)
        {
            return (from searchDataModel in dataModels
                where searchDataModel.EntryType == MediaEntryType.Anime
                select new Anime(searchDataModel)).Cast<T>();
        }

        private static IEnumerable<T> GetEntryList(IEnumerable<SearchDataModel> dataModels)
        {
            return (from searchDataModel in dataModels
                select
                searchDataModel.EntryType == MediaEntryType.Anime
                    ? new Anime(searchDataModel)
                    : (IMediaObject) new Manga(searchDataModel)).Cast<T>();
        }

        private static IEnumerable<T> GetMangaList(IEnumerable<SearchDataModel> dataModels)
        {
            return (from searchDataModel in dataModels
                where searchDataModel.EntryType == MediaEntryType.Manga
                select new Manga(searchDataModel)).Cast<T>();
        }

        internal override async Task<IProxerResult<IEnumerable<T>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<SearchDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ListRequestBuilder.EntrySearch(this._input, ResultsPerPage, nextPage))
                .ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<T>>(lResult.Exceptions);
            SearchDataModel[] lData = lResult.Result;

            if (typeof(T) == typeof(Anime)) return new ProxerResult<IEnumerable<T>>(GetAnimeList(lData));
            return typeof(T) == typeof(Manga)
                ? new ProxerResult<IEnumerable<T>>(GetMangaList(lData))
                : new ProxerResult<IEnumerable<T>>(GetEntryList(lData));
        }

        #endregion
    }
}