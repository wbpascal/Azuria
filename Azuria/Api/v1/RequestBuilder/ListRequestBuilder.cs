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
        /// </summary>
        /// <param name="input"></param>
        /// <param name="kat"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApiRequest<SearchDataModel[]> EntryList(EntryListInput input, string kat, int limit,
            int page)
        {
            return ApiRequest<SearchDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist"))
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("kat", kat)
                .WithPostArguments(SearchQueryBuilder.Build(input));
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApiRequest<SearchDataModel[]> EntrySearch(SearchInput input, int limit, int page)
        {
            return ApiRequest<SearchDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch"))
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString())
                .WithPostArguments(SearchQueryBuilder.Build(input));
        }

        #endregion
    }
}