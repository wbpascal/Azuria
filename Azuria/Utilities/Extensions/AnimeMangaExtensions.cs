using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Enums;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class AnimeMangaExtensions
    {
        #region

        internal static async Task<ProxerResult<AnimeMangaContentDataModel[]>> GetContentObjects(
            this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<ListInfoDataModel>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetListInfo(animeMangaObject.Id,
                        await animeMangaObject.ContentCount.GetObject(-1), senpai));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<AnimeMangaContentDataModel[]>(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return
                    new ProxerResult<AnimeMangaContentDataModel[]>(new[]
                    {new ProxerApiException(lResult.Result.ErrorCode)});
            ListInfoDataModel lData = lResult.Result.Data;

            return lData.EndIndex == -1
                ? new ProxerResult<AnimeMangaContentDataModel[]>(new Exception[0])
                : new ProxerResult<AnimeMangaContentDataModel[]>(lData.ContentObjects);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitAvailableLangApi(this IAnimeMangaObject animeMangaObject,
            Senpai senpai)
        {
            ProxerResult<ProxerInfoLanguageResponse> lResult =
                await
                    RequestHandler.ApiCustomRequest<ProxerInfoLanguageResponse>(
                        ApiRequestBuilder.BuildForGetLanguage(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});

            (animeMangaObject as Anime)?.AvailableLanguages.SetInitialisedObject(
                lResult.Result.Data.Cast<AnimeLanguage>());
            (animeMangaObject as Manga)?.AvailableLanguages.SetInitialisedObject(lResult.Result.Data.Cast<Language>());

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitGroupsApi(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<GroupDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetGroups(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});

            animeMangaObject.Groups.SetInitialisedObject(from groupDataModel in lResult.Result.Data
                select new Group(groupDataModel));

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitIndustryApi(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<PublisherDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetPublisher(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});

            animeMangaObject.Industry.SetInitialisedObject(from publisherDataModel in lResult.Result.Data
                select new Industry(publisherDataModel));

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitIsHContentApi(this IAnimeMangaObject animeMangaObject,
            Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<bool>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetGate(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});

            animeMangaObject.IsHContent.SetInitialisedObject(lResult.Result.Data);

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitMainInfoApi(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<EntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetEntry(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});
            EntryDataModel lDataModel = lResult.Result.Data;

            (animeMangaObject as Anime)?.AnimeTyp.SetInitialisedObject((AnimeType) lDataModel.Medium);
            animeMangaObject.Clicks.SetInitialisedObject(lDataModel.Clicks);
            animeMangaObject.ContentCount.SetInitialisedObject(lDataModel.ContentCount);
            animeMangaObject.Description.SetInitialisedObject(lDataModel.Description);
            animeMangaObject.Fsk.SetInitialisedObject(lDataModel.Fsk);
            animeMangaObject.Genre.SetInitialisedObject(lDataModel.Genre);
            animeMangaObject.IsLicensed.SetInitialisedObject(lDataModel.IsLicensed);
            (animeMangaObject as Manga)?.MangaType.SetInitialisedObject((MangaType) lDataModel.Medium);
            animeMangaObject.Name.SetInitialisedObject(lDataModel.Name);
            animeMangaObject.Rating.SetInitialisedObject(lDataModel.Rating);
            animeMangaObject.Status.SetInitialisedObject(lDataModel.State);

            return new ProxerResult();
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitNamesApi(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<NameDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetName(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});
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

        internal static async Task<ProxerResult> InitSeasonsApi(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<SeasonDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetSeason(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});
            SeasonDataModel[] lData = lResult.Result.Data;

            if (lData.Length > 1 && !lData[0].Equals(lData[1]))
                animeMangaObject.Season.SetInitialisedObject(new AnimeMangaSeasonInfo(lData[0], lData[1]));
            else if (lData.Length > 0) animeMangaObject.Season.SetInitialisedObject(new AnimeMangaSeasonInfo(lData[0]));

            return new ProxerResult();
        }

        #endregion
    }
}