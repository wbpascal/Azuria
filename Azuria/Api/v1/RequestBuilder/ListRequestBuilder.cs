using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.Converters.List;
using Azuria.Api.v1.DataModels.List;
using Azuria.Enums.Info;
using Azuria.Helpers.Search;
using Azuria.Input;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the list api class.
    /// </summary>
    public class ListRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public ListRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns a list of anime or manga that match
        /// the specified criteria.
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="input">The criteria that the returned anime or manga should match.</param>
        /// <param name="kat">
        /// Optional. Whether only anime or manga should be included in the returned list. Possible values:
        /// "anime", "manga". Default: "anime"
        /// </param>
        /// <param name="limit">
        /// Optional. The amount of anime or manga that will be returned per page. Default: 100
        /// </param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of search results.</returns>
        public IUrlBuilderWithResult<SearchDataModel[]> EntryList(
            EntryListInput input, string kat = "anime",
            int limit = 100, int page = 0)
        {
            return new UrlBuilder<SearchDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist"), this._client
                ).WithGetParameter("limit", limit.ToString())
                 .WithGetParameter("p", page.ToString())
                 .WithGetParameter("kat", kat)
                 .WithPostParameter(SearchQueryBuilder.Build(input));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the results of a search for anime and
        /// manga.
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="input">The criteria that will be searched for.</param>
        /// <param name="limit">
        /// Optional. The amount of anime and manga that will be returned per page. Default: 100
        /// </param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>
        /// An instance of <see cref="ApiRequest" /> that returns an array of search results.
        /// </returns>
        public IUrlBuilderWithResult<SearchDataModel[]> EntrySearch(
            SearchInput input, int limit = 100,
            int page = 0)
        {
            return new UrlBuilder<SearchDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch"), this._client
                ).WithGetParameter("limit", limit.ToString())
                 .WithGetParameter("p", page.ToString())
                 .WithPostParameter(SearchQueryBuilder.Build(input));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="start"></param>
        /// <param name="contains"></param>
        /// <param name="country"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns</returns>
        public IUrlBuilderWithResult<IndustryDataModel[]> GetIndustries(
            string start = "", string contains = "",
            string country = "", string type = "", int limit = 100, int page = 0)
        {
            return new UrlBuilder<IndustryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/industrys"), this._client
                ).WithGetParameter("start", start)
                 .WithGetParameter("contains", contains)
                 .WithGetParameter("country", country)
                 .WithGetParameter("type", type)
                 .WithGetParameter("limit", limit.ToString())
                 .WithGetParameter("p", page.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="translatorId"></param>
        /// <param name="type"></param>
        /// <param name="isH"></param>
        /// <param name="p"></param>
        /// <param name="limit"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns</returns>
        public IUrlBuilderWithResult<IndustryProjectDataModel[]> GetIndustryProjects(
            int translatorId,
            IndustryType? type = null, bool? isH = false, int p = 0, int limit = 100)
        {
            return new UrlBuilder<IndustryProjectDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/industryprojects"), this._client
                ).WithGetParameter("id", translatorId.ToString())
                 .WithGetParameter("type", type.ToString())
                 .WithGetParameter(
                     "isH", (isH is bool isHValue
                                 ? isHValue
                                       ? 1
                                       : -1
                                 : 0).ToString())
                 .WithGetParameter("p", p.ToString())
                 .WithGetParameter("limit", limit.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="search"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns</returns>
        public IUrlBuilderWithResult<Tuple<int[], int[]>> GetTagIds(string search)
        {
            return new UrlBuilder<Tuple<int[], int[]>>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/tagids"), this._client
                ).WithGetParameter("search", search)
                 .WithCustomDataConverter(new TagIdConverter());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="start"></param>
        /// <param name="contains"></param>
        /// <param name="country"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns</returns>
        public IUrlBuilderWithResult<TranslatorDataModel[]> GetTranslatorgroups(
            string start = "",
            string contains = "", string country = "", int limit = 100, int page = 0)
        {
            return new UrlBuilder<TranslatorDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroups"), this._client
                ).WithGetParameter("start", start)
                 .WithGetParameter("contains", contains)
                 .WithGetParameter("country", country)
                 .WithGetParameter("limit", limit.ToString())
                 .WithGetParameter("p", page.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns
        /// 
        /// Api permissions required:
        /// * List - Level 0
        /// </summary>
        /// <param name="translatorId"></param>
        /// <param name="type"></param>
        /// <param name="isH"></param>
        /// <param name="p"></param>
        /// <param name="limit"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns</returns>
        public IUrlBuilderWithResult<TranslatorProjectDataModel[]> GetTranslatorProjects(
            int translatorId,
            TranslationStatus? type = null, bool? isH = false, int p = 0, int limit = 100)
        {
            return new UrlBuilder<TranslatorProjectDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroupprojects"), this._client
                ).WithGetParameter("id", translatorId.ToString())
                 .WithGetParameter(
                     "type", type is TranslationStatus typeValue
                                 ? ((int) typeValue).ToString()
                                 : string.Empty)
                 .WithGetParameter(
                     "isH", (isH is bool isHValue
                                 ? isHValue
                                       ? 1
                                       : -1
                                 : 0).ToString())
                 .WithGetParameter("p", p.ToString())
                 .WithGetParameter("limit", limit.ToString());
        }

        #endregion
    }
}