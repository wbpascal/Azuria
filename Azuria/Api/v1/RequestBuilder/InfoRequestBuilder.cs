using System;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Input.Info;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the info api class.
    /// </summary>
    public class InfoRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public InfoRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }

        /// <summary>
        /// Builds a request that returns all comments of an anime or manga
        /// (with more than 300 characters).
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of comments.</returns>
        public IRequestBuilderWithResult<CommentDataModel[]> GetComments(CommentListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/comments"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns the core information of an anime or
        /// manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the core information.</returns>
        public IRequestBuilderWithResult<EntryDataModel> GetEntry(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<EntryDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/entry"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all tags of an anime or manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of tags.</returns>
        public IRequestBuilderWithResult<TagDataModel[]> GetEntryTags(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<TagDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/entrytags"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all informations of an anime or manga.
        /// **Warning!:**
        /// The returned object creates a heavy load on the server if it is used in a request!
        /// Be sure to only use it if you are certain that you need all returned informations!
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the informations.</returns>
        public IRequestBuilderWithResult<FullEntryDataModel> GetFullEntry(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<FullEntryDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/fullentry"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns a boolean indicating if the anime or
        /// manga requires an 18+ age check.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns a boolean.</returns>
        public IRequestBuilderWithResult<bool> GetGate(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<bool>(new Uri($"{ApiConstants.ApiUrlV1}/info/gate"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns informations about the translators of
        /// an anime or manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of translators.</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetGroups(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<TranslatorDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/groups"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns information about a company.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns information about a company.</returns>
        public IRequestBuilderWithResult<IndustryDataModel> GetIndustry(IndustryInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<IndustryDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/industry"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all languages an anime or manga is
        /// available in.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of languages.</returns>
        public IRequestBuilderWithResult<MediaLanguage[]> GetLanguage(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<MediaLanguage[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/lang"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithCustomDataConverter(new LanguageCollectionConverter());
        }

        /// <summary>
        /// Builds a request that returns all episodes or chapters of an anime
        /// or manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an object containing all chapter/episodes
        /// and information about the returned list.
        /// </returns>
        public IRequestBuilderWithResult<ListInfoDataModel> GetListInfo(ListInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ListInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/listinfo"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all names and synonymous of an anime
        /// or manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of object containing the names
        /// and some additional informations.
        /// </returns>
        public IRequestBuilderWithResult<NameDataModel[]> GetNames(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<NameDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/names"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns returns all organisations that were
        /// involved with creating or publishing the an anime or manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of organisations.
        /// </returns>
        public IRequestBuilderWithResult<IndustryBasicDataModel[]> GetPublisher(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<IndustryBasicDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/publisher"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all relations of an anime or manga.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of relations.</returns>
        public IRequestBuilderWithResult<RelationDataModel[]> GetRelations(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<RelationDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/relations"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns the seasons an anime or manga aired in.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of seasons.</returns>
        public IRequestBuilderWithResult<SeasonDataModel[]> GetSeason(EntryIdInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<SeasonDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/season"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns information about a translator group.
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns information about a translator group.</returns>
        public IRequestBuilderWithResult<TranslatorDataModel> GetTranslatorGroup(TranslatorInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<TranslatorDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/translatorgroup"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that adds an anime or manga to a list of a logged
        /// in user.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Info - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetUserInfo(SetUserInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/setuserinfo"), this.ProxerClient)
                .WithPostParameter(input.Build())
                .WithLoginCheck();
        }
    }
}