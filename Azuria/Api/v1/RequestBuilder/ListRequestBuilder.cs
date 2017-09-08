using System;
using System.Linq;
using Azuria.Api.v1.Converters.List;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.Input.List;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.List;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using TagDataModel = Azuria.Api.v1.DataModels.List.TagDataModel;

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
        /// <param name="input">The data model that contains further input parameters for the request.</param>
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
        /// <param name="input">The data model that contains further input parameters for the request.</param>
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
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<IndustryDataModel[]> GetIndustries(
            IndustryListInput input, int limit = 100, int page = 0)
        {
            return new RequestBuilder<IndustryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/industrys"), this.ProxerClient
                ).WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="p"></param>
        /// <param name="limit"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<IndustryProjectDataModel[]> GetIndustryProjects(
            IndustryProjectsInput input, int p = 0, int limit = 100)
        {
            return new RequestBuilder<IndustryProjectDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/industryprojects"), this.ProxerClient
                ).WithGetParameter("p", p.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter(input.Build());
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
        /// <param name="input"></param>
        /// <returns></returns>
        public IRequestBuilderWithResult<TagDataModel[]> GetTags(TagListInput input)
        {
            return new RequestBuilder<TagDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/tags"), this.ProxerClient
            ).WithGetParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetTranslatorgroups(
            TranslatorListInput input, int limit = 100, int page = 0)
        {
            return new RequestBuilder<TranslatorDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroups"), this.ProxerClient
                ).WithGetParameter("limit", limit.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="p"></param>
        /// <param name="limit"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<TranslatorProjectDataModel[]> GetTranslatorProjects(
            TranslatorProjectsInput input, int p = 0, int limit = 100)
        {
            return new RequestBuilder<TranslatorProjectDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroupprojects"), this.ProxerClient
                ).WithGetParameter("p", p.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter(input.Build());
        }
    }
}