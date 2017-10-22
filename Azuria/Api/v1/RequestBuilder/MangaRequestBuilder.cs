using System;
using Azuria.Api.v1.DataModels.Manga;
using Azuria.Api.v1.Input.Manga;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the manga api class.
    /// </summary>
    public class MangaRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public MangaRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }
        
        /// <summary>
        /// <para>
        /// Builds a request that returns information about a chapter including the pages.
        /// </para>
        /// <para>
        /// Api permissions required (class - permission level):
        /// * Manga - Level 2
        /// </para>
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the chapter.</returns>
        public IRequestBuilderWithResult<ChapterDataModel> GetChapter(ChapterInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ChapterDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/manga/chapter"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary());
        }
    }
}