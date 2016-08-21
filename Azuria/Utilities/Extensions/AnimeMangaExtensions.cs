using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Enums;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class AnimeMangaExtensions
    {
        #region

        internal static async Task<ProxerResult<AnimeMangaContentDataModel[]>> GetContentObjects(
            this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<ListInfoDataModel>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetListInfo(animeMangaObject.Id,
                        await animeMangaObject.ContentCount.GetObject(-1)));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<AnimeMangaContentDataModel[]>(lResult.Exceptions);
            ListInfoDataModel lData = lResult.Result.Data;

            return lData.EndIndex == -1
                ? new ProxerResult<AnimeMangaContentDataModel[]>(new Exception[0])
                : new ProxerResult<AnimeMangaContentDataModel[]>(lData.ContentObjects);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitAvailableLangApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerInfoLanguageResponse> lResult =
                await
                    RequestHandler.ApiCustomRequest<ProxerInfoLanguageResponse>(
                        ApiRequestBuilder.InfoGetLanguage(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            (animeMangaObject as Anime)?.AvailableLanguages.SetInitialisedObject(
                lResult.Result.Data.Cast<AnimeLanguage>());
            (animeMangaObject as Manga)?.AvailableLanguages.SetInitialisedObject(lResult.Result.Data.Cast<Language>());

            return new ProxerResult();
        }

        internal static async Task<ProxerResult> InitEntryTagsApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<EntryTagDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetEntryTags(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            animeMangaObject.Tags.SetInitialisedObject(from entryTagDataModel in lResult.Result.Data
                select new Tag(entryTagDataModel));
            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitGroupsApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<GroupDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetGroups(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            animeMangaObject.Groups.SetInitialisedObject(from groupDataModel in lResult.Result.Data
                select new Group(groupDataModel));

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitIndustryApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<PublisherDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetPublisher(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            animeMangaObject.Industry.SetInitialisedObject(from publisherDataModel in lResult.Result.Data
                select new Industry(publisherDataModel));

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitIsHContentApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<bool>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetGate(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            animeMangaObject.IsHContent.SetInitialisedObject(lResult.Result.Data);

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitMainInfoApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<EntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetEntry(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            EntryDataModel lDataModel = lResult.Result.Data;

            (animeMangaObject as Anime)?.AnimeMedium.SetInitialisedObject((AnimeMedium) lDataModel.EntryMedium);
            animeMangaObject.Clicks.SetInitialisedObject(lDataModel.Clicks);
            animeMangaObject.ContentCount.SetInitialisedObject(lDataModel.ContentCount);
            animeMangaObject.Description.SetInitialisedObject(lDataModel.Description);
            animeMangaObject.Fsk.SetInitialisedObject(lDataModel.Fsk);
            animeMangaObject.Genre.SetInitialisedObject(lDataModel.Genre);
            animeMangaObject.IsLicensed.SetInitialisedObject(lDataModel.IsLicensed);
            (animeMangaObject as Manga)?.MangaMedium.SetInitialisedObject((MangaMedium) lDataModel.EntryMedium);
            animeMangaObject.Name.SetInitialisedObject(lDataModel.EntryName);
            animeMangaObject.Rating.SetInitialisedObject(lDataModel.Rating);
            animeMangaObject.Status.SetInitialisedObject(lDataModel.Status);

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitNamesApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<NameDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetName(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            foreach (NameDataModel nameDataModel in lResult.Result.Data)
            {
                switch (nameDataModel.Type)
                {
                    case AnimeMangaNameType.Original:
                        animeMangaObject.Name.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case AnimeMangaNameType.English:
                        animeMangaObject.EnglishTitle.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case AnimeMangaNameType.German:
                        animeMangaObject.GermanTitle.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case AnimeMangaNameType.Japanese:
                        animeMangaObject.JapaneseTitle.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case AnimeMangaNameType.Synonym:
                        animeMangaObject.Synonym.SetInitialisedObject(nameDataModel.Name);
                        break;
                }
            }

            return new ProxerResult();
        }

        internal static async Task<ProxerResult> InitRelationsApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<RelationDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetRelations(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            animeMangaObject.Relations.SetInitialisedObject(from dataModel in lResult.Result.Data
                select
                    dataModel.EntryType == AnimeMangaEntryType.Anime
                        ? new Anime(dataModel)
                        : (IAnimeMangaObject) new Manga(dataModel));

            return new ProxerResult();
        }

        internal static async Task<ProxerResult> InitSeasonsApi(this IAnimeMangaObject animeMangaObject)
        {
            ProxerResult<ProxerApiResponse<SeasonDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetSeason(animeMangaObject.Id));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            SeasonDataModel[] lData = lResult.Result.Data;

            if (lData.Length > 1 && !lData[0].Equals(lData[1]))
                animeMangaObject.Season.SetInitialisedObject(new AnimeMangaSeasonInfo(lData[0], lData[1]));
            else if (lData.Length > 0) animeMangaObject.Season.SetInitialisedObject(new AnimeMangaSeasonInfo(lData[0]));

            return new ProxerResult();
        }

        #endregion
    }
}