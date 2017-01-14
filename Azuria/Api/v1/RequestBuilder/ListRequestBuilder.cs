using System;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Search;
using Azuria.Search.Input;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the list api class.
    /// </summary>
    public static class ListRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that returns a list of anime or manga that match the specified criteria.
        /// 
        /// Api permissions required:
        ///  * List - Level 0
        /// </summary>
        /// <param name="input">The criteria that the returned anime or manga should match.</param>
        /// <param name="kat">Optional. Whether only anime or manga should be included in the returned list. Possible values: "anime", "manga". Default: "anime"</param>
        /// <param name="limit">Optional. The amount of anime or manga that will be returned per page. Default: 100</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns an array of search results.</returns>
        public static ApiRequest<SearchDataModel[]> EntryList(EntryListInput input, string kat = "anime", int limit = 100,
            int page = 0)
        {
            return ApiRequest<SearchDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist?limit={limit}&p={page}&kat={kat}"))
                .WithPostArguments(SearchQueryBuilder.Build(input));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that returns the results of a search for anime and manga.
        /// 
        /// Api permissions required:
        ///  * List - Level 0
        /// </summary>
        /// <param name="input">The criteria that will be searched for.</param>
        /// <param name="limit">Optional. The amount of anime and manga that will be returned per page. Default: 100</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns an array of search results.</returns>
        public static ApiRequest<SearchDataModel[]> EntrySearch(SearchInput input, int limit = 100, int page = 0)
        {
            return ApiRequest<SearchDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch?limit={limit}&p={page}"))
                .WithPostArguments(SearchQueryBuilder.Build(input));
        }

        #endregion
    }
}