using System;
using System.Linq;
using Azuria.Api.v1.Converters.List;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.Input;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.List;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the list api class.
    /// </summary>
    public class ListRequestBuilder : IApiClassRequestBuilder
    {
        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        public ListRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <summary>
        /// Builds a request that returns the results of a search for anime and
        /// manga.
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="input">The criteria that will be searched for.</param>
        /// <param name="limit">
        /// Optional. The amount of anime and manga that will be returned per page. Default: 100
        /// </param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>
        /// An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of search results.
        /// </returns>
        public IRequestBuilderWithResult<SearchDataModel[]> EntrySearch(
            SearchInput input, int limit = 100, int page = 0)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return new RequestBuilder<SearchDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch"), this.ProxerClient
                ).WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString())
                .WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns a list of anime or manga that match
        /// the specified criteria.
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="input">The criteria that the returned anime or manga should match.</param>
        /// <param name="limit">
        /// Optional. The amount of anime or manga that will be returned per page. Default: 100
        /// </param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of search results.</returns>
        public IRequestBuilderWithResult<SearchDataModel[]> GetEntryList(
            EntryListInput input, int limit = 100, int page = 0)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return new RequestBuilder<SearchDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist"), this.ProxerClient
                ).WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString())
                .WithPostParameter(input.Build());
        }

        private static int GetHValue(bool? isH)
        {
            switch (isH)
            {
                case true:
                    return 1;
                case false:
                    return -1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="start"></param>
        /// <param name="contains"></param>
        /// <param name="country"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<IndustryDataModel[]> GetIndustries(
            string start = "", string contains = "", Country? country = null, IndustryType? type = null,
            int limit = 100, int page = 0)
        {
            if (country == Country.England)
                throw new ArgumentException(
                    "Country.England is not supported in this request! " +
                    "Please use Country.UnitedStates instead!", nameof(country)
                );
            if (type == IndustryType.Unknown)
                throw new ArgumentException("The given industry type is invalid for this request!", nameof(type));

            return new RequestBuilder<IndustryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/industrys"), this.ProxerClient
                ).WithGetParameter("start", start)
                .WithGetParameter("contains", contains)
                .WithGetParameter("country", country?.ToShortString() ?? string.Empty)
                .WithGetParameter("type", type?.ToTypeString() ?? string.Empty)
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="translatorId"></param>
        /// <param name="type"></param>
        /// <param name="isH"></param>
        /// <param name="p"></param>
        /// <param name="limit"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<IndustryProjectDataModel[]> GetIndustryProjects(
            int translatorId, IndustryType? type = null, bool? isH = false, int p = 0, int limit = 100)
        {
            if (type == IndustryType.Unknown)
                throw new ArgumentException("The given industry type is invalid for this request!", nameof(type));

            return new RequestBuilder<IndustryProjectDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/industryprojects"), this.ProxerClient
                ).WithGetParameter("id", translatorId.ToString())
                .WithGetParameter("type", type?.ToTypeString() ?? string.Empty)
                .WithGetParameter("isH", GetHValue(isH).ToString())
                .WithGetParameter("p", p.ToString())
                .WithGetParameter("limit", limit.ToString());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="tagsInclude"></param>
        /// <param name="tagsExclude"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<Tuple<int[], int[]>> GetTagIds(string[] tagsInclude, string[] tagsExclude)
        {
            if (tagsInclude == null)
                throw new ArgumentNullException(nameof(tagsInclude));
            if (tagsExclude == null)
                throw new ArgumentNullException(nameof(tagsExclude));

            string lSearch = tagsInclude.ToString(" ") + " " + tagsExclude.Aggregate(
                                 string.Empty, (s, s1) => string.Concat(s, " -", s1)).Trim();
            return new RequestBuilder<Tuple<int[], int[]>>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/tagids"), this.ProxerClient
                ).WithGetParameter("search", lSearch)
                .WithCustomDataConverter(new TagIdConverter());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="search"></param>
        /// <param name="type"></param>
        /// <param name="sort"></param>
        /// <param name="sortDirection"></param>
        /// <param name="subtype"></param>
        /// <returns></returns>
        public IRequestBuilderWithResult<TagDataModel[]> GetTags(
            string search = "", TagType? type = null, TagListSort sort = TagListSort.Tag,
            SortDirection sortDirection = SortDirection.Ascending, TagSubtype? subtype = null)
        {
            if (type == TagType.Unkown)
                throw new ArgumentException("The given tag type is invalid for this request!", nameof(type));
            if (subtype == TagSubtype.Unkown)
                throw new ArgumentException("The given tag subtype is invalid for this request!", nameof(subtype));

            return new RequestBuilder<TagDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/tags"), this.ProxerClient
                ).WithGetParameter("search", search)
                .WithGetParameter("type", type?.GetDescription() ?? string.Empty)
                .WithGetParameter("sort", sort.ToString().ToLowerInvariant())
                .WithGetParameter("sort_type", sortDirection.GetDescription())
                .WithGetParameter("subtype", subtype?.GetDescription() ?? string.Empty);
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="start"></param>
        /// <param name="contains"></param>
        /// <param name="country"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetTranslatorgroups(
            string start = "", string contains = "", Country? country = null, int limit = 100, int page = 0)
        {
            if (country == Country.Japan || country == Country.UnitedStates)
                throw new ArgumentException("The given country is invalid for this request!", nameof(country));

            return new RequestBuilder<TranslatorDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroups"), this.ProxerClient
                ).WithGetParameter("start", start)
                .WithGetParameter("contains", contains)
                .WithGetParameter("country", country?.ToShortString() ?? string.Empty)
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="translatorId"></param>
        /// <param name="type"></param>
        /// <param name="isH"></param>
        /// <param name="p"></param>
        /// <param name="limit"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<TranslatorProjectDataModel[]> GetTranslatorProjects(
            int translatorId, TranslationStatus? type = null, bool? isH = false, int p = 0, int limit = 100)
        {
            return new RequestBuilder<TranslatorProjectDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroupprojects"), this.ProxerClient
                ).WithGetParameter("id", translatorId.ToString())
                .WithGetParameter("type", type != null ? ((int) type.Value).ToString() : string.Empty)
                .WithGetParameter("isH", GetHValue(isH).ToString())
                .WithGetParameter("p", p.ToString())
                .WithGetParameter("limit", limit.ToString());
        }
    }
}