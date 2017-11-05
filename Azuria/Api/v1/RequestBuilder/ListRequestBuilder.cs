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
    public class ListRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public ListRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }

        /// <summary>
        /// Builds a request that returns the results of a search for anime and
        /// manga.
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of search results.
        /// </returns>
        public IRequestBuilderWithResult<SearchDataModel[]> EntrySearch(SearchInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<SearchDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns a list of anime or manga that match
        /// the specified criteria.
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of search results.</returns>
        public IRequestBuilderWithResult<SearchDataModel[]> GetEntryList(EntryListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<SearchDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<IndustryDataModel[]> GetIndustries(IndustryListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<IndustryDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/industrys"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<IndustryProjectDataModel[]> GetIndustryProjects(IndustryProjectsInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<IndustryProjectDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/industryprojects"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<Tuple<int[], int[]>> GetTagIds(TagIdSearchInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<Tuple<int[], int[]>>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/tagids"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithCustomDataConverter(new TagIdConverter());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns></returns>
        public IRequestBuilderWithResult<TagDataModel[]> GetTags(TagListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<TagDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/tags"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetTranslatorgroups(TranslatorListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<TranslatorDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroups"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that returns
        /// Api permissions required (class - permission level):
        /// * List - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns</returns>
        public IRequestBuilderWithResult<TranslatorProjectDataModel[]> GetTranslatorProjects(
            TranslatorProjectsInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<TranslatorProjectDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/list/translatorgroupprojects"), this.ProxerClient
            ).WithPostParameter(input.Build());
        }
    }
}