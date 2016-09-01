using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Api.v1.Enums;
using Azuria.Search.Input;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.Search
{
    internal class SearchResultEnumerator<T> : PageEnumerator<T> where T : IAnimeMangaObject
    {
        private const int ResultsPerPage = 100;
        private readonly SearchInput _input;

        internal SearchResultEnumerator(SearchInput input) : base(ResultsPerPage)
        {
            this._input = input;
        }

        #region

        private static IEnumerable<T> GetAnimeList(IEnumerable<SearchDataModel> dataModels)
        {
            return (from searchDataModel in dataModels
                where searchDataModel.EntryType == AnimeMangaEntryType.Anime
                select new Anime(searchDataModel)).Cast<T>();
        }

        private static IEnumerable<T> GetEntryList(IEnumerable<SearchDataModel> dataModels)
        {
            return (from searchDataModel in dataModels
                select
                searchDataModel.EntryType == AnimeMangaEntryType.Anime
                    ? new Anime(searchDataModel)
                    : (IAnimeMangaObject) new Manga(searchDataModel)).Cast<T>();
        }

        private static IEnumerable<T> GetMangaList(IEnumerable<SearchDataModel> dataModels)
        {
            return (from searchDataModel in dataModels
                where searchDataModel.EntryType == AnimeMangaEntryType.Manga
                select new Manga(searchDataModel)).Cast<T>();
        }

        internal override async Task<ProxerResult<IEnumerable<T>>> GetNextPage(int nextPage)
        {
            ProxerResult<ProxerApiResponse<SearchDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.SearchEntrySearch(this._input, ResultsPerPage, nextPage));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<T>>(lResult.Exceptions);
            SearchDataModel[] lData = lResult.Result.Data;

            if (typeof(T) == typeof(Anime)) return new ProxerResult<IEnumerable<T>>(GetAnimeList(lData));
            return typeof(T) == typeof(Manga)
                ? new ProxerResult<IEnumerable<T>>(GetMangaList(lData))
                : new ProxerResult<IEnumerable<T>>(GetEntryList(lData));
        }

        #endregion
    }
}