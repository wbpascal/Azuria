﻿using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Enums;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria
{
    /// <summary>
    ///     Some random functions that are to be moved later.
    /// </summary>
    public static class ProxerClass
    {
        #region

        /// <summary>
        ///     Gets an <see cref="Anime" /> or <see cref="Manga" /> of a specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="Anime" /> or <see cref="Manga" />.</param>
        /// <param name="senpai">The user that makes the request.</param>
        /// <returns>
        ///     If the action was successful and if it was, an object representing either an <see cref="Anime" /> or
        ///     <see cref="Manga" />.
        /// </returns>
        [ItemNotNull]
        public static async Task<ProxerResult<IAnimeMangaObject>> GetAnimeMangaById(int id, [NotNull] Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<EntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetEntry(id, senpai));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IAnimeMangaObject>(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult<IAnimeMangaObject>(new[] {new ProxerApiException(lResult.Result.ErrorCode)});
            EntryDataModel lDataModel = lResult.Result.Data;

            return
                new ProxerResult<IAnimeMangaObject>(lDataModel.EntryType == AnimeMangaEntryType.Anime
                    ? new Anime(lDataModel)
                    : (IAnimeMangaObject) new Manga(lDataModel));
        }

        #endregion
    }
}