using System;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Search;
using Azuria.Search.Input;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class ListRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that...
        /// 
        /// Api permissions required:
        ///  * List - Level 0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="kat"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns...</returns>
        public static ApiRequest<SearchDataModel[]> EntryList(EntryListInput input, string kat, int limit,
            int page)
        {
            return ApiRequest<SearchDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist?limit={limit}&p={page}&kat={kat}"))
                .WithPostArguments(SearchQueryBuilder.Build(input));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that...
        /// 
        /// Api permissions required:
        ///  * List - Level 0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns...</returns>
        public static ApiRequest<SearchDataModel[]> EntrySearch(SearchInput input, int limit, int page)
        {
            return ApiRequest<SearchDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch?limit={limit}&p={page}"))
                .WithPostArguments(SearchQueryBuilder.Build(input));
        }

        #endregion
    }
}